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

        public List<OrdersView> GetOrders (List<string> customerTypes)
        {
            string sql = $@"SELECT 
                            o.Accno,
                            o.IDNumber, 
                            CASE
                            WHEN c.CustomerType = '' THEN -1
                            ELSE TRY_CAST(c.CustomerType AS INT)
                            END AS CustomerType
                            From {DbTableName.Orders} As o
                            Inner Join {DbTableName.CustomerData} As c on o.IDnumber = c.Idnumber
                            Inner Join {DbTableName.Maincase} AS m ON o.Accno = m.AccNo
                            WHERE c.CustomerType in @customerTypes
                            And c.IDnumber NOT like '%~%'
                            ORDER BY o.IDNumber,
                            m.createDate";
            

            return connection.Query<OrdersView>(sql, new { customerTypes }, transaction).ToList();
        }

        public List<string> GetDuplicateOrderIds(List<string> idsToSearch)
        {
            string sql = $@"SELECT 
                            IDNumber
                            From {DbTableName.Orders}                            
                            WHERE IDnumber IN @Ids                            
                            GROUP BY IDNumber 
                            HAVING COUNT(IDNumber) > 1";
                
            return connection.Query<string>(sql, new { Ids = idsToSearch }, transaction).ToList();
        }

        /// <inheritdoc/>
        public int InsertNewIDNumber(CopyParam copyParam)
        {            
            string sql = $@"INSERT into {DbTableName.Orders} (AccNo, IDnumber) VALUES (@AccNo, @NewIDNumber)";
            return connection.Execute(sql, copyParam, transaction);                            
        }

        /// <inheritdoc/>
        public int Update(CopyParam copyParam)
        {
            string sql = $@"Update {DbTableName.Orders} 
                            Set Idnumber = '@NewIDNumber '
                            Where Accno ='@AccNo'
                            And Idnumber = '@IDNumber'";

            return connection.Execute(sql, copyParam, transaction);                
        }
    }
}
