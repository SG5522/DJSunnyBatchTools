using BatchIDnumber.Const;
using DBEntities.Const;
using Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace BatchIDnumber.Models
{
    public class ReportViewModel : CopyParam
    {
        /// <summary>
        /// 戶名
        /// </summary>
        [Display(Order = 3)]
        public string AccName { get; set; } = string.Empty;

        /// <summary>
        /// 身份別
        /// </summary>
        [Display(Order = 4)]
        public CustomerType CustomerType { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        [Display(Order = 5)]
        public IDNumberChangeResult Result { get; set; } = IDNumberChangeResult.Failure;
    }
}
