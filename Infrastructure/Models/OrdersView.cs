using DBEntities.Entities;
using DBEntities.Const;
using CommonLib.Extensions;

namespace Infrastructure.Models
{
    public class OrdersView : Orders
    {        
        /// <summary>
        /// 使用者
        /// </summary>
        public CustomerType CustomerType { get; set; }

        /// <summary>
        /// 是否重複
        /// </summary>
        public bool IsSingle { get; set; } = false;


        public override string ToString()
        {
            string customerTypeString = CustomerType.GetDescription();
            
            return $"AccNo: {AccNo}, IDNumber: {IDNumber}, CustomerType： {customerTypeString}";
        }
    }
}