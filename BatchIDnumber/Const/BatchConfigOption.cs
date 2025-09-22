namespace BatchIDnumber.Const
{
    /// <summary>
    /// 帳號清單
    /// </summary>
    public class BatchConfigOption
    {
        /// <summary>
        /// 檔案路徑
        /// </summary>
        public string DirectoryPath { get; set; } = string.Empty;

        /// <summary>
        /// 放置路徑
        /// </summary>
        public string AccListFileName { get; set; } = string.Empty;

        /// <summary>
        /// 完成後的報表
        /// </summary>
        public string ReportName { get; set; } = string.Empty;

        public string GetAccListPath()
            => Path.Combine(string.IsNullOrWhiteSpace(DirectoryPath) ? Directory.GetCurrentDirectory() : DirectoryPath, AccListFileName);

        public string GetReportPath()
            => Path.Combine(string.IsNullOrWhiteSpace(DirectoryPath) ? Directory.GetCurrentDirectory() : DirectoryPath, ReportName);
    }
}