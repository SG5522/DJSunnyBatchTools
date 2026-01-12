using BatchIDnumber.Const;
using BatchIDnumber.Models;
using BatchIDnumber.Service.Interface;
using CommonLib.Enums;
using CommonLib.Models;
using CommonLib.Utils;
using DJSpire.Models;
using DJSpire.Utils;
using ExcelChange;
using Infrastructure.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BatchIDnumber.Service.Implement
{
    public class ReportService : IReportService
    {        
        private readonly IBatchQueryService batchQueryService;
        private readonly ILogger<ReportService> logger;
        private BatchConfigOption batchConfigOption;

        /// <summary>
        /// 建置
        /// </summary>
        /// <param name="iDNumberBatchService"></param>
        /// <param name="batchQueryService"></param>
        /// <param name="logger">Log</param>
        /// <param name="optionsMonitor">appsetting的BatchConfigOption參數</param>
        public ReportService(IBatchQueryService batchQueryService, ILogger<ReportService> logger, IOptionsMonitor<BatchConfigOption> optionsMonitor)
        {            
            this.batchQueryService = batchQueryService;
            this.logger = logger;
            batchConfigOption = optionsMonitor.CurrentValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountRecords"></param>
        /// <returns></returns>
        public async Task CreateIgnoreListExcelReport(List<AccountRecord> accountRecords)
        {            
            try
            {
                List<ReportIgnoreList> reportIgnoreLists = accountRecords.Where(x =>
                {
                    ValidationResult validationResult = TaiwanIdValidator.Validate(x.IDNumber);
                    return validationResult.Type == IdType.UnifiedBusinessNumber || validationResult.Type == IdType.Unknown;
                }).Select(reportIL => new ReportIgnoreList
                {
                    IDNumber = reportIL.IDNumber,
                    AccNo = reportIL.AccNo,                    
                }).ToList();
                
                foreach (ReportIgnoreList reportIgnoreList in reportIgnoreLists)
                {
                    reportIgnoreList.AccName = await batchQueryService.GetAccountName(reportIgnoreList.AccNo);
                }

                CreateExcelReport(reportIgnoreLists, ExcelHearderConsts.ReportIgnore, batchConfigOption.GetReportIgnorePath());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "生成忽略清單的Excel 報表時發生錯誤。");
            }
        }

        /// <inheritdoc/>
        public void CreateExcelReport<T>(List<T> report, List<string> excelHearder, string savePath) where T : class
        {
            // 如果沒有資料需要寫入，可以提前返回
            if (report.Count == 0)
            {
                logger.LogWarning("沒有資料需要生成 {TypeName} Excel 報表。", typeof(T).Name);
                return;
            }

            try
            {
                ExcelData<T> excelData = new(report, excelHearder);                
                ExcelUtil.CreateFileToSavePath(excelData, savePath);
                logger.LogInformation("{TypeName} Excel 報表已成功儲存至：{filePath}", typeof(T).Name, savePath);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "生成 {TypeName} Excel 報表時發生錯誤。", typeof(T).Name);
            }
        }
    }
}
