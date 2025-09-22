using BatchIDnumber.Const;
using BatchIDnumber.Models;
using BatchIDnumber.Service.Interface;
using CommonLib.Enums;
using CommonLib.Utils;
using DBEntities.Entities;
using DJSpire.Models;
using DJSpire.Utils;
using Infrastructure.Models;
using Infrastructure.Repository.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BatchIDnumber.Service.Implement
{
    public class IDNumberBatchService : IIDNumberBatchService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<IDNumberBatchService> logger;
        private BatchConfigOption batchConfigOption;

        /// <summary>
        /// 建置
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="">logger</param>
        public IDNumberBatchService(IUnitOfWork unitOfWork, ILogger<IDNumberBatchService> logger, IOptionsMonitor<BatchConfigOption> optionsMonitor)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            batchConfigOption = optionsMonitor.CurrentValue;
        }

        public async Task<List<OrdersView>> GetOrders(List<string> customerTypes)
        {
            return await unitOfWork.OrdersRepository.GetOrders(customerTypes);
        }

        /// <summary>
        /// 批次處理
        /// </summary>
        /// <param name="ordersViewList"></param>
        public async Task Process(List<OrdersView> ordersViewList)
        {
            try
            {
                //過濾掉統編為公司戶的清單
                ordersViewList = ordersViewList.Where(x => TaiwanIdValidator.Validate(x.IDNumber).Type != IdType.UnifiedBusinessNumber).ToList();
                await MarkOrderSingularity(ordersViewList);
                List<ReportViewModel> reports = new();
                
                foreach (OrdersView ordersView in ordersViewList.Where(o => o.IsSingle))
                {
                    reports.Add(await ProcessSingleOrders(ordersView));
                }

                foreach (IGrouping<string, OrdersView> group in ordersViewList.Where(o => !o.IsSingle).GroupBy(o => o.IDNumber))
                {
                    int idnumberCount = 0;
                    foreach (OrdersView order in group)
                    {
                        reports.Add(await ProcessDuplicateOrder(order, ++idnumberCount));
                    }
                }

                unitOfWork.Commit();
                CreateExcelReport(reports);
            }
            catch (Exception ex)
            {
                logger.LogError($"Process Error {ex.Message}");
                unitOfWork.Rollback();
            }            
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

                report.Result = IDNumberChangeResult.Success;
                await SetReportAccName(report);
            }
            catch (Exception ex)
            {                
                logger.LogError("帳號：{@AccNo} 處理失敗，在動作 {Action} 時發生錯誤 {@ErrorMessage} ", ordersView.AccNo, currentAction, ex.Message);
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
                List<OldPhoto> oldPhotos = await unitOfWork.OldPhotoRepository.GetOldPhotos(report.IDNumber);
                List<OldIdentifycard> oldIdentifycards = await unitOfWork.OldIdentifycardRepository.GetOldIdentifycards(report.IDNumber);

                currentAction = "CustomerDataRepository.CopyWithNewId";
                await unitOfWork.CustomerDataRepository.CopyWithNewId(report);

                currentAction = "OrdersRepository.InsertNewIDNumber";
                await unitOfWork.OrdersRepository.InsertNewIDNumber(report);

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

                report.Result = IDNumberChangeResult.Success;
                await SetReportAccName(report);
            }
            catch (Exception ex)
            {
                logger.LogError("帳號：{@AccNo} 處理失敗，在動作 {Action} 時發生錯誤 {@ErrorMessage} ", ordersView.AccNo, currentAction, ex.Message);
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

            List<string> duplicateIds = await unitOfWork.OrdersRepository.GetDuplicateOrderIds(uniqueIds);

            HashSet<string> duplicateIdsSet = new(duplicateIds);

            foreach (OrdersView order in partialOrders)
            {
                order.IsSingle = !duplicateIdsSet.Contains(order.IDNumber);
            }
        }

        private async Task SetReportAccName(ReportViewModel report)
           => report.AccName = await unitOfWork.MainCaseRepository.GetAccName(report.AccNo) ?? "主表找不到資料";
        

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
