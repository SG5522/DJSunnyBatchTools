using Dapper;
using DBEntities.Entities;
using Infrastructure.Models;
using Infrastructure.Repository.Interface;
using System.Data;

namespace Infrastructure.Repository.Implement
{
    public class AccTestRepository : IAccTestRepository
    {
        private readonly IDbConnection connection;
        private readonly IDbTransaction transaction;
        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public AccTestRepository(IDbConnection connection, IDbTransaction transaction)
        {
            this.connection = connection;
            this.transaction = transaction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task<int> InsertAccTest(AccountRecord accountList)
        {            
            AccTest accTest = new ()
            {
                AccNo = accountList.AccNo,
                IDNumber = accountList.IDNumber,
                CustomerType = accountList.CustomerType
            };

            string sql = $@"INSERT into AccTest
                        (Accno, IdNumber, CustomerType) VALUES
                        (@Accno,
                        @IDNumber,                        
                        @CustomerType)";

            return await connection.ExecuteAsync(sql, accTest, transaction);
        }

        public List<AccTest> GetAccTest()
        {
            string sql = $@"Select * From AccTest";

            return connection.Query<AccTest>(sql, null, transaction).ToList();
        }
    }
}
