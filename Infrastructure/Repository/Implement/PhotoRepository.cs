using Dapper;
using DBEntities.Const;
using DBEntities.Entities;
using Infrastructure.Models;
using Infrastructure.Repository.Interface;
using System.Data;

namespace Infrastructure.Repository.Implement
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly IDbConnection connection;
        private readonly IDbTransaction transaction;        

        public PhotoRepository(IDbConnection connection, IDbTransaction transaction)
        {
            this.connection = connection;
            this.transaction = transaction;            
        }

        public async Task<bool> CheckPhoto (string idNumber)
        {
            string sql = $@"SELECT 1 FROM {DbTableName.Photo} Where IDNumber = @IDNumber";

            return await connection.QueryFirstOrDefaultAsync<int>(sql, new { IDNumber = idNumber }, transaction) != 0;
        }

        /// <inheritdoc/>
        public async Task<int> Insert(Photo photo)
        {
            string sql = $@"INSERT into {DbTableName.Photo} (IDnumber, ImagePath, featureJson, [DateTime]) 
                            VALUES (@IDnumber, @ImagePath, @FeatureJson, @DateTime)";

            return await connection.ExecuteAsync(sql, photo, transaction);            
        }

        /// <inheritdoc/>
        public async Task<int> InsertCopy(CopyParam copyParam)
        {
            string sql = $@"INSERT into {DbTableName.Photo} (IDnumber, ImagePath, featureJson, [DateTime]) 
                            SELECT 
                            @NewIDNumber AS IDnumber,
                            old.ImagePath,
                            old.featureJson,           
                            old.[DateTime]
                            FROM {DbTableName.Photo} AS old
                            WHERE IDNumber = @IDNumber";

            return await connection.ExecuteAsync(sql, copyParam, transaction);
        }

        /// <inheritdoc/>
        public async Task<int> Update(Photo photo)
        {            
            string sql = $@"Update {DbTableName.Photo} 
                            Set ImagePath = @ImagePath,
                                featureJson = @FeatureJson,
                                [DateTime] = @DateTime
                            WHERE IDnumber = @IDNumber";

            return await connection.ExecuteAsync(sql, photo, transaction);
        }

        /// <inheritdoc/>
        public async Task<int> UpdateIDNumber(CopyParam copyParam)
        {
            int insertCount = await InsertCopy(copyParam);
            int deleteCount = await Delete(copyParam.IDNumber);

            return insertCount + deleteCount;
        }        

        /// <summary>
        /// 刪除人像
        /// </summary>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        private async Task<int> Delete (string idNumber)
        {
            string sql = $@"DELETE FROM {DbTableName.Photo} 
                            WHERE IDnumber = @IDNumber";

            return await connection.ExecuteAsync(sql, new { IDNumber = idNumber }, transaction);            
        }
    }
}
