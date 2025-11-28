using DBEntities.Const;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    /// <summary>
    /// 複製資料的參數
    /// </summary>
    public class CopyParam : BaseParam
    {
        /// <summary>
        /// 身份別
        /// </summary>
        [Display(Order = 4)]
        public CustomerType NewCustomerType { get; set; }

        /// <summary>
        /// 變更後的統編
        /// </summary>        
        public string NewIDNumber { get; set; } = string.Empty;
    }
}
