namespace BatchIDnumber.Const
{
    /// <summary>
    /// 儲存資料庫中所有資料表的名稱常數。
    /// </summary>
    public class ExcelHearderConsts
    {
        /// <summary>
        /// 批次完成後的結果報表
        /// </summary>
        public static readonly List<string> BatchIDNumberChange = new()
        {
            "帳號",
            "統編",
            "戶名",
            "身份別",
            "執行結果"
        };

        public static readonly List<string> ReportIgnore = new()
        {
            "帳號",
            "統編",
            "戶名"
        };
    }    
}
