using Dapper;
using DBEntities.Const;
using System.Data;

namespace Infrastructure.Repository.Implement
{
    public class MainCaseRepository : IMainCaseRepository
    {
        private readonly IDbConnection connection;
        private readonly IDbTransaction transaction;        

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public MainCaseRepository(IDbConnection connection, IDbTransaction transaction)
        {
            this.connection = connection;
            this.transaction = transaction;            
        }

        /// <inheritdoc/>
        public string? GetAccName(string accNo)
        {
            string sql = $@"SELECT 
                            AccName
                            From {DbTableName.Maincase}                            
                            WHERE AccNo = @AccNo";

            return connection.QueryFirstOrDefault<string>(sql, new { AccNo = accNo }, transaction);
        }

    }
}
