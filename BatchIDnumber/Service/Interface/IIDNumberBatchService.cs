

using BatchIDnumber.Models;
using Infrastructure.Models;

namespace BatchIDnumber.Service.Interface
{
    public interface IIDNumberBatchService
    {
        /// <summary>
        /// 依照身份別取得Orders清單
        /// </summary>
        /// <param name="customerTypes"></param>
        /// <returns></returns>
        Task<List<AccountRecord>> GetOrders(List<string> customerTypes);

        /// <summary>
        /// 變更籌備處、聯名戶的統編
        /// </summary>
        /// <param name="ordersViews"></param>
        /// <returns></returns>
        Task Process(List<AccountRecord> ordersViews);

    }
}
