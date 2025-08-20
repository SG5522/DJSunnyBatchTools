using Dapper;
using DBEntities.Const;
using DBEntities.Entities;
using Infrastructure.Models;
using Infrastructure.Repository.Interface;
using System.Data;

namespace Infrastructure.Repository.Implement
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly IDbConnection connection;
        private readonly IDbTransaction transaction;        

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>        
        public OrdersRepository(IDbConnection connection, IDbTransaction transaction)
        {
            this.connection = connection;
            this.transaction = transaction;            
        }

        /// <inheritdoc/>
        public int Insert(Orders orders)
        {            
            string sql = $@"INSERT into {DbTableName.Orders} (AccNo, IDnumber) VALUES (@AccNo, @IDNumber)";
            return connection.Execute(sql, orders, transaction);                            
        }

        /// <inheritdoc/>
        public int Update(OrdersView ordersView)
        {

            string sql = $@"Update {DbTableName.Orders} 
                            Set Idnumber = '@NewIDNumber '
                            Where Accno ='@AccNo'
                            And Idnumber = '@IDNumber'";

            return connection.Execute(sql, ordersView, transaction);                
        }
    }
}
