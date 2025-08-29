using Dapper;
using DBEntities.Const;
using DBEntities.Entities;
using Infrastructure.Models;
using Infrastructure.Repository.Interface;
using Infrastructure.Utils;
using System.Data;

namespace Infrastructure.Repository.Implement
{

    public class OldPhotoRepository : IOldPhotoRepository
    {
        private readonly IDbConnection connection;
        private readonly IDbTransaction transaction;        

        public OldPhotoRepository(IDbConnection connection, IDbTransaction transaction)
        {
            this.connection = connection;
            this.transaction = transaction;            
        }

        /// <inheritdoc/>
        public List<OldPhoto> GetOldPhotos(string idNumber)
        {
            string sql = $@"SELECT * FROM oldidentifycard
                            WHERE IDnumber = @idNumber";

            return connection.Query<OldPhoto>(sql, new { idNumber }).ToList();
        }

        /// <inheritdoc/>
        public int Inserts(List<OldPhoto> oldPhotos, string idNumber)
        {
            int result = default;
              
            foreach (OldPhoto oldPhoto in oldPhotos)
            {
                oldPhoto.Sn = StringUtil.GetRandomSN();
                oldPhoto.Idnumber = idNumber;
                result += Insert(oldPhoto);
            }
            //logger.LogInformation("IDnumber {@idNumber} 身份證歷史紀錄加入 :{@insertConut} 筆", idNumber, insertConut); 靠service層紀錄
            return result;
        }

        /// <inheritdoc/>
        public int Insert(OldPhoto oldPhoto)
        {
            string sql = $@"INSERT into {DbTableName.OldPhoto} (IDnumber, ImagePath, featureJson, [DateTime], SN) 
                            VALUES (@IDnumber, @ImagePath, @FeatureJson, @DateTime, @Sn)";

            return connection.Execute(sql, oldPhoto, transaction);
        }

        /// <inheritdoc/>
        public int UpdateIDNumber(CopyParam copyParam)
        {
            string sql = $@"Update {DbTableName.OldPhoto} 
                            Set IDnumber = @NewIDNumber,
                            WHERE IDnumber = @IDNumber ";

            return connection.Execute(sql, copyParam, transaction);
        }
    }
}
