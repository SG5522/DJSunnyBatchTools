using DBEntities.Const;

namespace DBEntities.Entities
{
    public class AccTest
    {
        /// <summary>
        /// 帳號
        /// </summary>        
        public string AccNo { get; set; } = string.Empty;

        /// <summary>
        /// 統編
        /// </summary>
        public string IDNumber { get; set; } = string.Empty;

        /// <summary>
        /// 身份別
        /// </summary>
        public CustomerType CustomerType { get; set; }
    }
}
