
using System.ComponentModel;


namespace DBEntities.Const
{
    public enum CustomerType
    {
        /// <summary>空字串</summary>
        [Description("空字串")]
        Empty = -1,

        /// <summary>0. 無</summary>
        [Description("無")]
        None = 0,
            
        /// <summary>1. 身分證</summary>
        [Description("身分證")]
        IDCardNumber = 1,

        /// <summary>2. 戶口名簿(未成年無身分證)</summary>
        [Description("戶口名簿(未成年無身分證)")]
        HouseBook = 2,

        /// <summary>3. 居留證(外國人)</summary>
        [Description("居留證(外國人)")]
        ResidentPermit = 3,

        /// <summary>4. 統一編號(公司戶)</summary>
        [Description("統一編號(公司戶)")]
        TaxIDNumber = 4,

        /// <summary>5. 籌備處</summary>
        [Description("籌備處")]
        Preparation = 5,

        // <summary>6. 聯名戶</summary>
        [Description("聯名戶")]
        JointName = 6,

        /// <summary>99. 其他</summary>
        [Description("其他")]
        Other = 99,
    }    
}
