using DBEntities.Const;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    /// <summary>
    /// 帶入資料庫查詢、新增、更新用各項參數
    /// </summary>
    public class BaseParam
    {
        /// <summary>
        /// 要變更的帳號
        /// </summary>
        [Display(Order = 1)]
        public string AccNo { get; set; } = string.Empty;

        /// <summary>
        /// 來源統編
        /// </summary>
        [Display(Order = 2)]
        public string IDNumber { get; set; } = string.Empty;
    }
}
