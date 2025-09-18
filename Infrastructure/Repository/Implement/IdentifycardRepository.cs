using Dapper;
using DBEntities.Const;
using DBEntities.Entities;
using Infrastructure.Models;
using Infrastructure.Repository.Interface;
using System.Data;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Implement
{
    public class IdentifycardRepository : IIdentifycardRepository
    {
        private readonly IDbConnection connection;
        private readonly IDbTransaction transaction;        

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>        
        public IdentifycardRepository(IDbConnection connection, IDbTransaction transaction)
        {
            this.connection = connection;
            this.transaction = transaction;            
        }

        /// <inheritdoc/>
        public async Task<int> Insert(IdentifyCard identifycard)
        {
            string sql = $@"INSERT into {DbTableName.IdentifyCard} (IDnumber, IMAGEFPATH, [Order], [DateTime], housebook) 
                            VALUES (@IDnumber, @Imagefpath, @Order, @DateTime, @Housebook)";

            return await connection.ExecuteAsync(sql, identifycard, transaction);            
        }

        /// <inheritdoc/>
        public async Task<int> InsertCopy(CopyParam copyParam)
        {
            string sql = $@"INSERT into {DbTableName.IdentifyCard} (IDnumber, IMAGEFPATH, [Order], [DateTime], housebook) 
                            SELECT 
                            @NewIDNumber AS IDnumber,
                            old.IMAGEFPATH,
                            old.[Order],           
                            old.[DateTime],
                            old.housebook
                            FROM {DbTableName.IdentifyCard} AS old
                            WHERE IDNumber = @IDNumber ";

            return await connection.ExecuteAsync(sql, copyParam, transaction);
        }

        /// <inheritdoc/>
        public async Task<int> UpdateIDNumber(CopyParam copyParam)
        {
            string sql = $@"Update {DbTableName.IdentifyCard} 
                            Set IDnumber = @NewIDNumber 
                            WHERE IDnumber = @IDNumber ";

            return await connection.ExecuteAsync(sql, copyParam, transaction);            
        }
    }
}
