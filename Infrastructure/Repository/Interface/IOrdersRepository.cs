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
        List<OrdersView> GetOrders(List<string> customerTypes);

        /// <summary>
        /// 從資料庫中找出在給定清單裡有重複紀錄的 IDNumber。
        /// </summary>
        /// <param name="idsToSearch">需要查詢的 IDNumber 清單。</param>
        /// <returns>重複的 IDNumber 清單。</returns>
        List<string> GetDuplicateOrderIds(List<string> idsToSearch);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="copyParam"></param>
        /// <returns></returns>
        int InsertNewIDNumber(CopyParam copyParam);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="copyParam"></param>
        /// <returns></returns>
        int Update(CopyParam copyParam);
    }
}
