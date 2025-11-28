using Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace BatchIDnumber.Models
{
    public class ReportIgnoreList : BaseParam
    {
        /// <summary>
        /// 戶名
        /// </summary>
        [Display(Order = 3)]
        public string AccName { get; set; } = string.Empty;
    }
}
