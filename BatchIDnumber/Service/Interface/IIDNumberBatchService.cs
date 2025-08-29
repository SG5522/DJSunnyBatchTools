

using Infrastructure.Models;

namespace BatchIDnumber.Service.Interface
{
    public interface IIDNumberBatchService
    {
        List<OrdersView> GetOrders(List<string> customerTypes);

        void Process(List<OrdersView> ordersViews);
    }
}
