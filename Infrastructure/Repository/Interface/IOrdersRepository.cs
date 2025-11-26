using Infrastructure.Models;

namespace Infrastructure.Repository.Interface
{
    public interface IOrdersRepository
    {
        /// <summary>
        /// 依照customrdata 的customerType 取得Idnumber不含"~"的Orders清單
        /// </summary>
        /// <param name="customerTypes"></param>
        /// <returns></returns>
        Task<List<AccountRecord>> GetOrders(List<string> customerTypes);

        /// <summary>
        /// 從資料庫中找出在給定清單裡有重複紀錄的 IDNumber。
        /// </summary>
        /// <param name="idsToSearch">需要查詢的 IDNumber 清單。</param>
        /// <returns>重複的 IDNumber 清單。</returns>
        Task<List<string>> GetDuplicateOrderIds(List<string> idsToSearch);

        /// <summary>
        /// 檢查 Orders 表中特定 ID/AccNo 的資料是否存在，並確保其 AccNo 存在於 Maincase 中。
        /// </summary>
        /// <param name="copyParam">包含 AccNo 和 IDNumber 的參數。</param>
        /// <returns>如果找到符合條件且在 Maincase 中有對應 AccNo 的記錄，則返回 true。</returns>
        Task<bool> CheckOrders(CopyParam copyParam);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="copyParam"></param>
        /// <returns></returns>
        Task<int> InsertNewIDNumber(CopyParam copyParam);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="copyParam"></param>
        /// <returns></returns>
        Task<int> Update(CopyParam copyParam);
    }
}
