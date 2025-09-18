using Dapper;
using DBEntities.Const;
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
        public async Task<int> InsertNewIDNumber(CopyParam copyParam)
        {
            string sql = $@"INSERT into {DbTableName.Combinid} (AccNo, IDnumber, BranchId) VALUES (@AccNo, @NewIDNumber, @BranchId)";
            return await connection.ExecuteAsync(sql, copyParam, transaction);
        }

        /// <inheritdoc/>
        public async Task<int> InsertCopy(CopyParam copyParam)
        {
            string sql = $@"INSERT into {DbTableName.Combinid} (AccNo, IDnumber, BranchId) 
                            SELECT 
                            oldCombinid.AccNo, 
                            @NewIdnumber AS IDnumber, 
                            oldCombinid.BranchId 
                        FROM {DbTableName.Combinid} AS oldCombinid 
                        WHERE oldCombinid.AccNo = @AccNo 
                        AND oldCombinid.IDnumber = @IDNumber ";                        
            return await connection.ExecuteAsync(sql, copyParam, transaction);                    
        }


        /// <inheritdoc/>
        public async Task<int> Update(CopyParam copyParam)
        {
            string sql = $@"Update {DbTableName.Combinid} 
                            Set Idnumber = @NewIDNumber 
                            Where Accno = @AccNo 
                            AND IDnumber = @IDNumber ";

            return await connection.ExecuteAsync(sql, copyParam, transaction);                         
        }
    }
}
