using DBEntities.Entities;
using DBEntities.Const;
using CommonLib.Extensions;

namespace Infrastructure.Models
{
    /// <summary>
    /// 載入的Orders表與customerData join後獲得的資料
    /// </summary>
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

        /// <summary>
        /// 隨機身份別 謹限 Preparation、JointName
        /// </summary>
        public void RandomCustomerType()
        {
            CustomerType[] customerTypes = { CustomerType.Preparation, CustomerType.JointName };
            Random random = new();
            
            int randomIndex = random.Next(customerTypes.Length);

            CustomerType = customerTypes[randomIndex];
        }
    }
}