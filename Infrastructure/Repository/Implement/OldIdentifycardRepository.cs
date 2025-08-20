using Dapper;
using DBEntities.Const;
using DBEntities.Entities;
using Infrastructure.Repository.Interface;
using Infrastructure.Utils;
using Microsoft.Extensions.Logging;
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
        public int Inserts(List<OldIdentifycard> oldIdentifycards, string idNumber)
        {            
            int result = 0;
            foreach (OldIdentifycard oldIdentifycard in oldIdentifycards)
            {
                oldIdentifycard.Sn = StringUtil.GetRandomSn();
                oldIdentifycard.Idnumber = idNumber;
                result += Insert(oldIdentifycard);
            }
            //logger.LogInformation("IDnumber {@idNumber} 身份證歷史紀錄加入 :{@insertConut} 筆", idNumber, insertConut); 靠serveice層紀錄
            return result;                
        }

        /// <inheritdoc/>
        public int Insert(OldIdentifycard oldIdentifycard)
        {
            string sql = $@"INSERT into {DbTableName.Oldidentifycard} (IDnumber, IMAGEFPATH, Order, [DateTime], housebook, SN) 
                            VALUES (@IDnumber, @Imagefpath, @Order, @DateTime, @Housebook, @Sn)";                
                
            return connection.Execute(sql, oldIdentifycard, transaction);
        }
        
    }
}
