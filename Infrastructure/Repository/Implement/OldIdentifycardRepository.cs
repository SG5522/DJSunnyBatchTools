using Dapper;
using DBEntities.Const;
using DBEntities.Entities;
using Infrastructure.Models;
using Infrastructure.Repository.Interface;
using Infrastructure.Utils;
using System.Data;

namespace Infrastructure.Repository.Implement
{
    public class OldIdentifycardRepository : IOldIdentifycardRepository
    {
        private readonly IDbConnection connection;
        private readonly IDbTransaction transaction;        

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="logger"></param>
        public OldIdentifycardRepository(IDbConnection connection, IDbTransaction transaction)
        {
            this.connection = connection;
            this.transaction = transaction;
        }

        /// <inheritdoc/>
        public async Task<List<OldIdentifycard>> GetOldIdentifycards(string idNumber)
        {
            string sql = $@"SELECT * FROM {DbTableName.Oldidentifycard}
                            WHERE IDnumber = @idNumber";

            return (await connection.QueryAsync<OldIdentifycard>(sql, new { idNumber }, transaction)).ToList();
        }

        /// <inheritdoc/>
        public async Task<int> Inserts(List<OldIdentifycard> oldIdentifycards, string newIDNumber)
        {            
            int result = 0;
            foreach (OldIdentifycard oldIdentifycard in oldIdentifycards)
            {
                oldIdentifycard.Sn = StringUtil.GetRandomSN();
                oldIdentifycard.Idnumber = newIDNumber;
                result += await Insert(oldIdentifycard);
            }            
            return result;                
        }

        /// <inheritdoc/>
        public async Task<int> Insert(OldIdentifycard oldIdentifycard)
        {
            string sql = $@"INSERT into {DbTableName.Oldidentifycard} (IDnumber, IMAGEFPATH, [Order], [DateTime], housebook, SN)
                            VALUES (@IDnumber, @Imagefpath, @Order, @DateTime, @Housebook, @Sn)";
            
            return await connection.ExecuteAsync(sql, oldIdentifycard, transaction);
        }


        /// <inheritdoc/>
        public async Task<int> UpdateIDNumber(CopyParam copyParam)
        {
            string sql = $@"Update {DbTableName.Oldidentifycard} 
                            Set IDnumber = @NewIDNumber 
                            WHERE IDnumber = @IDNumber ";

            return await connection.ExecuteAsync(sql, copyParam, transaction);
        }
    }
}
