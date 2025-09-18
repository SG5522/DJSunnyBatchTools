

using Infrastructure.Models;

namespace BatchIDnumber.Service.Interface
{
    public interface IIDNumberBatchService
    {
        Task<List<OrdersView>> GetOrders(List<string> customerTypes);

        Task Process(List<OrdersView> ordersViews);
    }
}
