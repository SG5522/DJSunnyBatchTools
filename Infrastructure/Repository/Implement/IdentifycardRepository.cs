using Dapper;
using DBEntities.Const;
using DBEntities.Entities;
using Infrastructure.Repository.Interface;
using Microsoft.Extensions.Logging;
using System.Data;

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
        public int Insert(Identifycard identifycard)
        {
            string sql = $@"INSERT into {DbTableName.Identifycard} (IDnumber, IMAGEFPATH, Order, [DateTime], housebook) 
                            VALUES (@IDnumber, @Imagefpath, @Order, @DateTime, @Housebook)";

            return connection.Execute(sql, identifycard, transaction);            
        }
    }
}
