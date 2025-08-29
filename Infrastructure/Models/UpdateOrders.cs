using DBEntities.Entities;

namespace Infrastructure.Models
{
    public class UpdateOrders : Orders
    {        
        /// <summary>
        /// 更新後的Idnumber
        /// </summary>
        public string NewIDNumber { get; set; } = string.Empty;
    }
}
