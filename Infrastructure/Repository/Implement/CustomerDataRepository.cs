using Dapper;
using DBEntities.Const;
using DBEntities.Entities;
using Infrastructure.Repository.Interface;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Infrastructure.Repository.Implement
{
    public class CustomerDataRepository : ICustomerDataRepository
    {
        private readonly IDbConnection connection;
        private readonly IDbTransaction transaction;
        
        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="logger"></param>
        public CustomerDataRepository(IDbConnection connection, IDbTransaction transaction)
        {
            this.connection = connection;
            this.transaction = transaction;            
        }

        private class CustomerParam
        {
            public string NewIdnumber { get; set; } = string.Empty;
            public string Idnumber { get; set; } = string.Empty;
        }

        /// <inheritdoc/>
        public int Insert(Customerdata customerdata)
        {

            string sql = $@"INSERT into {DbTableName.Customerdata}
                        (Idnumber, Name, Sex, Birthday, Bplace, 
                        Issued, HomeAddress, MailAddress, TelPhone, MobilePhone, 
                        Email, OccupationCode, IsTempId, CustomerType) VALUES  
                        (@Idnumber, 
                        @Name, 
                        @Sex, 
                        @Birthday, 
                        @Bplace, 
                        @Issued, 
                        @HomeAddress, 
                        @MailAddress, 
                        @TelPhone, 
                        @MobilePhone, 
                        @Email, 
                        @OccupationCode, 
                        @IsTempId,
                        @CustomerType)";

            return connection.Execute(sql, customerdata, transaction);            
        }

        /// <inheritdoc/>
        public int InsertWithOldIdnumber(string idNumber, int count)
        {    
            string sql = $@"INSERT into {DbTableName.Customerdata}
                        (Idnumber, Name, Sex, Birthday, Bplace, 
                        Issued, HomeAddress, MailAddress, TelPhone, MobilePhone, 
                        Email, OccupationCode, IsTempId, CustomerType) VALUES  
                        SELECT 
                            @NewSqlIdnumber AS IDnumber,
                            oldCust.Name,     
                            oldCust.Sex,
                            oldCust.Birthday,
                            oldCust.Bplace,
                            oldCust.Issued,
                            oldCust.HomeAddress,
                            oldCust.MailAddress,
                            oldCust.TelPhone,
                            oldCust.MobilePhone,
                            oldCust.Email,
                            oldCust.OccupationCode,
                            oldCust.IsTempId,
                            oldCust.CustomerType,                        
                        FROM Customerdata AS oldCust
                        WHERE SN = '@Idnumber';";

            CustomerParam customerParam = new()
            {
                NewIdnumber = $"{idNumber}~{count}",
                Idnumber = idNumber
            };

            return connection.Execute(sql, customerParam, transaction);
        }

    }
}
