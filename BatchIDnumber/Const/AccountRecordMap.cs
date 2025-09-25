using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using DBEntities.Const;
using Infrastructure.Models;

namespace BatchIDnumber.Const
{
    public sealed class AccountRecordMap : ClassMap<AccountRecord>
    {
        public AccountRecordMap()
        {
            Map(m => m.AccNo);
            Map(m => m.IDNumber);
            //將Enum轉成數字
            Map(m => m.CustomerType).TypeConverterOption.Format("D");
        }
    }    
}

