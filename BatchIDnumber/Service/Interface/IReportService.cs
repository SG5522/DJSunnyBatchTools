

using Infrastructure.Models;

namespace BatchIDnumber.Service.Interface
{
    public interface IReportService
    {
        /// <summary>
        /// 建立忽略處理的資料清單並做成Excel
        /// </summary>
        /// <param name="accountRecords"></param>
        /// <returns></returns>
        Task CreateIgnoreListExcelReport(List<AccountRecord> accountRecords);

        /// <summary>
        /// Excel 報表的生成與儲存
        /// </summary>
        /// <param name="report">包含 Excel 資料的 List</param>
        /// <param name="excelHearder">Excel Hearder</param>
        /// <param name="savePath">存檔位置</param>
        void CreateExcelReport<T>(List<T> report, List<string> excelHearder, string savePath) where T : class;
    }
}
