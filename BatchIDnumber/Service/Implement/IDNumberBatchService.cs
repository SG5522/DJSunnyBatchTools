using BatchIDnumber.Const;
using BatchIDnumber.Models;
using BatchIDnumber.Service.Interface;
using CommonLib.Enums;
using CommonLib.Models;
using CommonLib.Utils;
using DBEntities.Entities;
using DJSpire.Models;
using DJSpire.Utils;
using Infrastructure.Models;
using Infrastructure.Repository.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;

namespace BatchIDnumber.Service.Implement
{
    public class IDNumberBatchService : IIDNumberBatchService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IReportService reportService;
        private readonly IBatchQueryService batchQueryService;
        private readonly ILogger<IDNumberBatchService> logger;        
        private BatchConfigOption batchConfigOption;

        /// <summary>
        /// 建置
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="">logger</param>
        public IDNumberBatchService(IUnitOfWork unitOfWork,
            IReportService reportService,
            IBatchQueryService batchQueryService,
            ILogger<IDNumberBatchService> logger, 
            IOptionsMonitor<BatchConfigOption> optionsMonitor)
        {
            this.unitOfWork = unitOfWork;
            this.reportService = reportService;
            this.batchQueryService = batchQueryService;
            this.logger = logger;
            batchConfigOption = optionsMonitor.CurrentValue;
        }

        public async Task<List<AccountRecord>> GetOrders(List<string> customerTypes)
        {
            return await unitOfWork.OrdersRepository.GetOrders(customerTypes);
        }

        /// <summary>
        /// 批次處理
        /// </summary>
        /// <param name="accountRecords"></param>
        public async Task Process(List<AccountRecord> accountRecords)
        {
            try
            {
                Console.WriteLine("列出忽略清單至Excel...");
                await reportService.CreateIgnoreListExcelReport(accountRecords);
                // 將 List<AccountList> 轉換為 List<OrdersView>
                List<OrdersView> ordersViewList = accountRecords
                        .Where(x => {
                            ValidationResult validationResult = TaiwanIdValidator.Validate(x.IDNumber);
                            return validationResult.Type != IdType.UnifiedBusinessNumber && validationResult.Type != IdType.Unknown;
                        }) //過濾掉統編為公司戶的清單以及包含#的統編
                        .Select(acc => new OrdersView
                        {
                            AccNo = acc.AccNo,
                            IDNumber = acc.IDNumber,
                            CustomerType = acc.CustomerType,
                            IsSingle = false // 初始化 IsSingle 屬性
                        }).ToList();

                Console.WriteLine("標記所有只有單一統編的帳號");
                await MarkOrderSingularity(ordersViewList);
                List<ReportViewModel> reports = new();

                Console.WriteLine("執行身分別調整與相關資料表的統編調整...");
                Console.WriteLine("處理單一統編的帳號...");
                foreach (OrdersView ordersView in ordersViewList.Where(o => o.IsSingle))
                {
                    reports.Add(await ProcessSingleOrders(ordersView));
                }

                Console.WriteLine("處理複數統編的帳號...");
                foreach (IGrouping<string, OrdersView> group in ordersViewList.Where(o => !o.IsSingle).GroupBy(o => o.IDNumber))
                {
                    int idnumberCount = 0;
                    foreach (OrdersView order in group)
                    {
                        reports.Add(await ProcessDuplicateOrder(order, ++idnumberCount));
                    }
                }

                Console.WriteLine("送入資料庫處理中...");                
                unitOfWork.Commit();
                reportService.CreateExcelReport(reports, ExcelHearderConsts.BatchIDNumberChange, batchConfigOption.GetReportPath());                
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Process Error");
                unitOfWork.Rollback();
            }            
        }

        public async Task CreateAccTestTxt(string filePath = "AccTestOutput.txt")
        {
            List<AccTest> accTests = unitOfWork.AccTestRepository.GetAccTest();                    

            // 使用 StringBuilder 來高效地建立大字串
            StringBuilder sb = new();

            foreach (var accTest in accTests)
            {
                // ... (同上步驟 4, 5, 6 格式化邏輯) ...
                string firstSegment = accTest.AccNo.Length >= 3 ? accTest.AccNo.Substring(0, 3) : string.Empty;
                string secondSegment = accTest.AccNo;
                string thirdSegment = accTest.IDNumber;
                int customerTypeInt = (int)accTest.CustomerType;

                string line = $"{firstSegment} {secondSegment} {thirdSegment.PadRight(15)} {customerTypeInt}";

                // 加入字串並換行
                sb.AppendLine(line);
            }

            // 非同步將整個大字串寫入檔案
            await File.WriteAllTextAsync(filePath, sb.ToString(), Encoding.UTF8);
        }      

        public async Task CreateTestData(List<AccountRecord> accountRecords)
        {
            foreach (var accountRecord in accountRecords)
            {
                await unitOfWork.AccTestRepository.InsertAccTest(accountRecord);
            }
            unitOfWork.Commit();
        }

        /// <summary>
        /// Orders的IDNumber只有一筆帳號時所做的批次處理
        /// </summary>
        /// <param name="ordersView"></param>
        private async Task<ReportViewModel> ProcessSingleOrders(OrdersView ordersView)
        {
            ReportViewModel report = new()
            {
                AccNo = ordersView.AccNo,
                IDNumber = ordersView.IDNumber,
                NewCustomerType = ordersView.CustomerType,
                NewIDNumber = $"{ordersView.IDNumber}{"~1"}"
            };

            string currentAction = string.Empty;

            try
            {
                //確認orders是否存在該帳號以及Idnumber                
                if (await unitOfWork.OrdersRepository.CheckOrders(report))
                {
                    currentAction = "CustomerDataRepository.UpdateIDAndType";
                    await unitOfWork.CustomerDataRepository.UpdateIDAndType(report);

                    currentAction = "CustomerDataRepository.Delete";
                    await unitOfWork.CustomerDataRepository.Delete(report.IDNumber);

                    currentAction = "OrdersRepository.Update";
                    await unitOfWork.OrdersRepository.Update(report);

                    currentAction = "CombinidRepository.Update";
                    await unitOfWork.CombinidRepository.Update(report);

                    currentAction = "PhotoRepository.UpdateIDNumber";
                    await unitOfWork.PhotoRepository.UpdateIDNumber(report);

                    currentAction = "IdentifycardRepository.UpdateIDNumber";
                    await unitOfWork.IdentifycardRepository.UpdateIDNumber(report);

                    currentAction = "OldPhotoRepository.UpdateIDNumber";
                    await unitOfWork.OldPhotoRepository.UpdateIDNumber(report);

                    currentAction = "OldIdentifycardRepository.UpdateIDNumber";
                    await unitOfWork.OldIdentifycardRepository.UpdateIDNumber(report);

                    report.AccName =  await batchQueryService.GetAccountName(report.AccNo);
                    report.Result = IDNumberChangeResult.Success;
                }             
            }
            catch (Exception ex)
            {                
                logger.LogError(ex, "帳號：{@AccNo} 處理失敗，在動作 {Action} 時發生錯誤 ", ordersView.AccNo, currentAction);
            }
            
            return report;
        }

        /// <summary>
        /// Orders的IDNumber有多筆帳號時所做的批次處理
        /// </summary>
        /// <param name="ordersView"></param>
        /// <param name="idNumberCount"></param>
        private async Task<ReportViewModel> ProcessDuplicateOrder(OrdersView ordersView, int idNumberCount)
        {
            ReportViewModel report = new()
            {
                AccNo = ordersView.AccNo,
                IDNumber = ordersView.IDNumber,
                NewCustomerType = ordersView.CustomerType,
                NewIDNumber = $"{ordersView.IDNumber}{"~"}{idNumberCount}",                
            };

            string currentAction = string.Empty;

            try
            {
                if (await unitOfWork.OrdersRepository.CheckOrders(report))
                {
                    List<OldPhoto> oldPhotos = await unitOfWork.OldPhotoRepository.GetOldPhotos(report.IDNumber);
                    List<OldIdentifycard> oldIdentifycards = await unitOfWork.OldIdentifycardRepository.GetOldIdentifycards(report.IDNumber);

                    currentAction = "CustomerDataRepository.CopyWithNewId";
                    await unitOfWork.CustomerDataRepository.CopyWithNewId(report);

                    currentAction = "OrdersRepository.Update";
                    await unitOfWork.OrdersRepository.Update(report);

                    currentAction = "CombinidRepository.InsertCopy";
                    await unitOfWork.CombinidRepository.InsertCopy(report);

                    currentAction = "PhotoRepository.InsertCopy";
                    await unitOfWork.PhotoRepository.InsertCopy(report);

                    currentAction = "IdentifycardRepository.InsertCopy";
                    await unitOfWork.IdentifycardRepository.InsertCopy(report);

                    currentAction = "OldPhotoRepository.Inserts";
                    await unitOfWork.OldPhotoRepository.Inserts(oldPhotos, report.NewIDNumber);

                    currentAction = "OldIdentifycardRepository.Inserts";
                    await unitOfWork.OldIdentifycardRepository.Inserts(oldIdentifycards, report.NewIDNumber);

                    report.AccName = await batchQueryService.GetAccountName(report.AccNo);
                    report.Result = IDNumberChangeResult.Success;
                    
                }                
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "帳號：{@AccNo} 處理失敗，在動作 {Action} 時發生錯誤", ordersView.AccNo, currentAction);
            }

            return report;
        }
        
        /// <summary>
        /// 處理Orders清單，並標記其中的重複項目。
        /// </summary>
        /// <param name="partialOrders">需要處理的訂單清單。</param>
        private async Task MarkOrderSingularity(List<OrdersView> partialOrders)
        {            
            List<string> uniqueIds = partialOrders.Select(o => o.IDNumber).Distinct().ToList();

            HashSet<string> allDuplicateIds = new();
            int batchSize = 2000;

            // 將 uniqueIds 清單分批，每批 2000 個            
            foreach (var batch in uniqueIds.Chunk(batchSize))
            {
                // 針對每一批 ID 呼叫資料庫
                List<string> duplicateIdsInBatch = await unitOfWork.OrdersRepository.GetDuplicateOrderIds(batch.ToList());

                // 將結果加入總清單中
                foreach (string id in duplicateIdsInBatch)
                {
                    allDuplicateIds.Add(id);
                }
            }

            partialOrders.ForEach(order =>
            {
                order.IsSingle = !allDuplicateIds.Contains(order.IDNumber);
            });
        }
        

        /// <summary>
        /// 獨立處理 Excel 報表的生成與儲存
        /// </summary>
        /// <param name="report">包含 Excel 資料的 List</param>
        private void CreateExcelReport(List<ReportViewModel> report)
        {
            // 如果沒有資料需要寫入，可以提前返回
            if (!report.Any())
            {
                logger.LogWarning("沒有資料需要生成 Excel 報表。");
                return;
            }

            try
            {
                ExcelData<ReportViewModel> excelData = new(report, ExcelHearderConsts.BatchIDNumberChange);                
                string filePath = batchConfigOption.GetReportPath();
                ExcelUtil.CreateFileToSavePath(excelData, filePath);
                logger.LogInformation("Excel 報表已成功儲存至：{filePath}", filePath);
            }
            catch (Exception ex)
            {
                logger.LogError($"生成 Excel 報表時發生錯誤：{ex.Message}");
            }
        }
    }
}
