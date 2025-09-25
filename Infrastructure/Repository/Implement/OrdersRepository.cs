using Dapper;
using DBEntities.Const;
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

        public async Task<List<AccountRecord>> GetOrders (List<string> customerTypes)
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
            
            return (await connection.QueryAsync<AccountRecord>(sql, new { customerTypes }, transaction)).ToList();
        }

        /// <inheritdoc/>
        public async Task<List<string>> GetDuplicateOrderIds(List<string> idsToSearch)
        {
            string sql = $@"SELECT 
                            IDNumber 
                            From {DbTableName.Orders} 
                            WHERE IDnumber IN @Ids";
                
            return (await connection.QueryAsync<string>(sql, new { Ids = idsToSearch }, transaction)).ToList();
        }

        /// <inheritdoc/>
        public async Task<int> InsertNewIDNumber(CopyParam copyParam)
        {            
            string sql = $@"INSERT into {DbTableName.Orders} (AccNo, IDnumber) VALUES (@AccNo, @NewIDNumber)";
            return await connection.ExecuteAsync(sql, copyParam, transaction);                            
        }

        /// <inheritdoc/>
        public async Task<int> Update(CopyParam copyParam)
        {
            string sql = $@"Update {DbTableName.Orders}
                            Set Idnumber = @NewIDNumber 
                            Where Accno = @AccNo 
                            And Idnumber = @IDNumber ";

            return await connection.ExecuteAsync(sql, copyParam, transaction);                
        }
    }
}
