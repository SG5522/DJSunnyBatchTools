

using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    /// <summary>
    /// 複製資料的參數
    /// </summary>
    public class CopyParam
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

        /// <summary>
        /// 變更後的統編
        /// </summary>        
        public string NewIDNumber { get; set; } = string.Empty;
    }
}
