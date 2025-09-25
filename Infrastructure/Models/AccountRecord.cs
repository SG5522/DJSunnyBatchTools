using CommonLib.Extensions;
using DBEntities.Const;
using DBEntities.Entities;

namespace Infrastructure.Models
{
    public class AccountRecord : Orders
    {
        /// <summary>
        /// 身份別
        /// </summary>
        public CustomerType CustomerType { get; set; }

        public override string ToString()
        {
            string customerTypeString = CustomerType.GetDescription();

            return $"AccNo: {AccNo}, IDNumber: {IDNumber}, CustomerType： {customerTypeString}";
        }

        /// <summary>
        /// 隨機身份別
        /// </summary>
        public void RandomCustomerType()
        {                        
            CustomerType[] customerTypes = (CustomerType[])Enum.GetValues(typeof(CustomerType));

            Random random = new();

            int randomIndex = random.Next(customerTypes.Length);

            CustomerType = customerTypes[randomIndex];
        }
    }
}
