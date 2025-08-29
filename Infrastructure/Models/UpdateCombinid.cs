using DBEntities.Entities;

namespace Infrastructure.Models
{
    public class UpdateCombinid : Combinid
    {        
        /// <summary>
        /// 更新後的Idnumber
        /// </summary>
        public string NewIDNumber { get; set; } = string.Empty;
    }
}
