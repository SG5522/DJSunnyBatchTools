using Dapper;
using DBEntities.Const;
using DBEntities.Entities;
using Infrastructure.Models;
using Infrastructure.Repository.Interface;
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
        public int InsertNewIDNumber(CopyParam copyParam)
        {
            string sql = $@"INSERT into {DbTableName.Combinid} (AccNo, IDnumber, BranchId) VALUES (@AccNo, @NewIDNumber, @BranchId)";
            return connection.Execute(sql, copyParam, transaction);
            //logger.LogInformation($"新增 Combinid 筆數為： {insertCount} "); 之後加到服務中            
        }

        /// <inheritdoc/>
        public int InsertCopy(CopyParam copyParam)
        {
            string sql = $@"INSERT into {DbTableName.Combinid} (AccNo, IDnumber, BranchId) 
                            SELECT 
                            oldCombinid.AccNo,
                            @NewIdnumber AS IDnumber,
                            oldCombinid.BranchId
                        FROM {DbTableName.Combinid} AS oldCombinid
                        WHERE oldCombinid.AccNo = @AccNo
                        AND oldCombinid.IDnumber = @IDNumber";                        
            return connection.Execute(sql, copyParam, transaction);                    
        }


        /// <inheritdoc/>
        public int Update(CopyParam copyParam)
        {
            string sql = $@"Update {DbTableName.Combinid} 
                            Set Idnumber = '@NewIDNumber'
                            Where Accno = '@AccNo'
                            AND IDnumber = '@IDNumber'";

            return connection.Execute(sql, copyParam, transaction);                         
        }
    }
}
