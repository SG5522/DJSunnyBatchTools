using Dapper;
using DBEntities.Const;
using DBEntities.Entities;
using Infrastructure.Models;
using Infrastructure.Repository.Interface;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Infrastructure.Repository.Implement
{
    public class CombinidRepository : ICombinidRepository
    {
        private readonly IDbConnection connection;
        private readonly IDbTransaction transaction;        

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public CombinidRepository(IDbConnection connection, IDbTransaction transaction)
        {
            this.connection = connection;
            this.transaction = transaction;            
        }

        /// <inheritdoc/>
        public bool Insert(Combinid combinid)
        {
            string sql = $@"INSERT into {DbTableName.Combinid} (AccNo, IDnumber, BranchId) VALUES (@AccNo, @IDNumber, @BranchId)";
            int insertCount = connection.Execute(sql, combinid, transaction);
            //logger.LogInformation($"新增 Combinid 筆數為： {insertCount} "); 之後加到服務中
            return true;
        }

        /// <inheritdoc/>
        public bool Update(CombinidView combinid)
        {
            string sql = $@"Update {DbTableName.Combinid} 
                            Set Idnumber = '@NewIDNumber'
                            Where Accno = '@AccNo'
                            AND IDnumber = '@IDNumber'";

            connection.Execute(sql, combinid, transaction);
            return true;            
        }
    }
}
