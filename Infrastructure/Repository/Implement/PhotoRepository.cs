using Dapper;
using DBEntities.Const;
using DBEntities.Entities;
using Infrastructure.Repository.Interface;
using Microsoft.Extensions.Logging;
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

        /// <inheritdoc/>
        public int InsertPhoto(Photo photo)
        {

            string sql = $@"INSERT into {DbTableName.Photo} (IDnumber, ImagePath, featureJson, [DateTime]) 
                            VALUES (@IDnumber, @ImagePath, @FeatureJson, @DateTime)";

            return connection.Execute(sql, photo, transaction);            
        }
    }
}
