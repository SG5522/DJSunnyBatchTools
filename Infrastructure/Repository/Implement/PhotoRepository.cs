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

        public bool CheckPhoto (string idNumber)
        {
            string sql = $@"SELECT 1 FROM {DbTableName.Photo} Where IDNumber = @IDNumber";

            return connection.QueryFirstOrDefault<int>(sql, new { IDNumber = idNumber }, transaction) != 0;
        }

        /// <inheritdoc/>
        public int Insert(Photo photo)
        {
            string sql = $@"INSERT into {DbTableName.Photo} (IDnumber, ImagePath, featureJson, [DateTime]) 
                            VALUES (@IDnumber, @ImagePath, @FeatureJson, @DateTime)";

            return connection.Execute(sql, photo, transaction);            
        }

        /// <inheritdoc/>
        public int InsertCopy(CopyParam copyParam)
        {
            string sql = $@"INSERT into {DbTableName.Photo} (IDnumber, ImagePath, featureJson, [DateTime]) 
                            SELECT 
                            @NewIdnumber AS IDnumber,
                            old.ImagePath,
                            old.featureJson,           
                            old.[DateTime]
                            FROM {DbTableName.Photo} AS old
                            WHERE IDNumber = @IdNumber";

            return connection.Execute(sql, copyParam, transaction);
        }

        /// <inheritdoc/>
        public int Update(Photo photo)
        {            
            string sql = $@"Update {DbTableName.Photo} 
                            Set ImagePath = @ImagePath,
                                featureJson = @FeatureJson,
                                [DateTime] = @DateTime
                            WHERE IDnumber = @IDNumber";

            return connection.Execute(sql, photo, transaction);
        }

        /// <inheritdoc/>
        public int UpdateIDNumber(CopyParam copyParam)
        {
            int insertCount = InsertCopy(copyParam);
            int deleteCount = Delete(copyParam.IDNumber);

            return insertCount + deleteCount;
        }        

        /// <summary>
        /// 刪除人像
        /// </summary>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        private int Delete (string idNumber)
        {
            string sql = $@"DELETE FROM {DbTableName.Photo} 
                            WHERE IDnumber = @idNumber";

            return connection.Execute(sql, idNumber, transaction);            
        }
    }
}
