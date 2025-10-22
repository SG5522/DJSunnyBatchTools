using DBEntities.Const;
using Infrastructure.Models;

namespace ExcelChange
{
    public class SunnyBankAccno
    {
        /// <summary>
        /// 分行單位(代號)
        /// </summary>
        public string BankID { get; set; } = null!;

        //帳號
        public string SubID { get; set; } = null!;

        //帳號
        public string SAccNo { get; set; } = null!;

        /// <summary>
        /// 帳號
        /// </summary>
        public string IDNumber { get; set; } = null!;

        /// <summary>
        /// 身份別
        /// </summary>
        public CustomerType CustomerType { get; set; }
    }
}
