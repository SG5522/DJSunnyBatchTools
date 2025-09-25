namespace Infrastructure.Models
{
    /// <summary>
    /// 載入的Orders表與customerData join後獲得的資料
    /// </summary>
    public class OrdersView : AccountRecord
    {
        /// <summary>
        /// 是否重複
        /// </summary>
        public bool IsSingle { get; set; } = false;
    }
}