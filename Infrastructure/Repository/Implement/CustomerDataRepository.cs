using Dapper;
using DBEntities.Const;
using DBEntities.Entities;
using Infrastructure.Models;
using Infrastructure.Repository.Interface;
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

        /// <inheritdoc/>
        public int Insert(Customerdata customerdata)
        {
            string sql = $@"INSERT into {DbTableName.CustomerData}
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
        public int CopyWithNewId(CopyParam copyParam)
        {    
            string sql = $@"INSERT into {DbTableName.CustomerData}
                        (Idnumber, Name, Sex, Birthday, Bplace,
                        Issued, HomeAddress, MailAddress, TelPhone, MobilePhone,
                        Email, OccupationCode, CustomerType)
                        SELECT
                            @NewIdnumber AS IDnumber,
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
                            oldCust.CustomerType
                        FROM Customerdata AS oldCust
                        WHERE oldCust.IDnumber = @IDNumber";

            return connection.Execute(sql, copyParam, transaction);
        }

        /// <inheritdoc/>
        public int Delete(string idNumber)
        {
            string sql = $@"Delete FROM {DbTableName.CustomerData}
                        WHERE IDnumber = @IDNumber";

            return connection.Execute(sql, new { IDNumber = idNumber }, transaction);

        }
    }
}
