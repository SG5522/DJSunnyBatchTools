namespace BatchIDnumber.Models
{
    public class ProcessCount
    {
        /// <summary>
        /// 新增客戶資料筆數
        /// </summary>
        public int InsertCustomerDataCount { get; set; }

        /// <summary>
        /// 刪除客戶資料筆數
        /// </summary>
        public int DeleteCustomerDataCount { get; set; }

        /// <summary>
        /// 更新客戶資料筆數
        /// </summary>
        public int UpdateCustomerDataCount { get; set; }

        /// <summary>
        /// 新增Orders資料筆數
        /// </summary>
        public int InsertOrdersCount { get; set; }

        /// <summary>
        /// 更新Orders資料筆數
        /// </summary>
        public int UpdateOrdersCount { get; set; }

        /// <summary>
        /// 新增Combinid資料筆數
        /// </summary>
        public int InsertCombinidCount { get; set; }

        /// <summary>
        /// 更新Combinid資料筆數
        /// </summary>
        public int UpdateCombinidCount { get; set; }

        /// <summary>
        /// 更新人像資料筆數
        /// </summary>
        public int InsertPhotoCount { get; set; }

        /// <summary>
        /// 更新人像資料筆數
        /// </summary>
        public int UpdatePhotoCount { get; set; }

        /// <summary>
        /// 新增證件資料筆數
        /// </summary>
        public int InsertIdentifycardCount { get; set; }

        /// <summary>
        /// 更新證件資料筆數
        /// </summary>
        public int UpdateIdentifycardCount { get; set; }

        /// <summary>
        /// 新增歷史人像資料筆數
        /// </summary>
        public int InsertOldPhotoCount { get; set; }

        /// <summary>
        /// 更新歷史人像資料筆數
        /// </summary>
        public int UpdateOldPhotoCount { get; set; }

        /// <summary>
        /// 新增歷史證件資料筆數
        /// </summary>
        public int InsertOldIdentifycardCount { get; set; }

        /// <summary>
        /// 更新歷史證件資料筆數
        /// </summary>
        public int UpdateOldIdentifycardCount { get; set; }

    }
}
