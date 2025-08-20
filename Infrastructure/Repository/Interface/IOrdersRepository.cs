using DBEntities.Entities;
using Infrastructure.Models;

namespace Infrastructure.Repository.Interface
{
    public interface IOrdersRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="orders"></param>
        /// <returns></returns>
        int Insert(Orders orders);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="ordersView"></param>
        /// <returns></returns>
        int Update(OrdersView ordersView);
    }
}
