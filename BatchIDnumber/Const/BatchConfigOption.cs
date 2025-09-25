namespace BatchIDnumber.Const
{
    /// <summary>
    /// 帳號清單
    /// </summary>
    public class BatchConfigOption
    {        
        private string accountListFileName = string.Empty;

        /// <summary>
        /// 檔案路徑
        /// </summary>
        public string DirectoryPath { get; set; } = string.Empty;

        /// <summary>
        /// 放置路徑
        /// </summary>
        public string AccountListFileName
        {
            get => accountListFileName;
            set
            {
                accountListFileName = value;
                // Automatically update FileFormat when the file name is set.
                FileFormat = GetFileFormat(value);
            }
        }

        /// <summary>
        /// 完成後的報表
        /// </summary>
        public string ReportFileName { get; set; } = string.Empty;

        /// <summary>
        /// 檔案格式
        /// </summary>
        public FileFormat FileFormat { get; private set; } = FileFormat.Unknown;

        public string GetAccListPath()
            => Path.Combine(string.IsNullOrWhiteSpace(DirectoryPath) ? Directory.GetCurrentDirectory() : DirectoryPath, AccountListFileName);

        public string GetReportPath()
            => Path.Combine(string.IsNullOrWhiteSpace(DirectoryPath) ? Directory.GetCurrentDirectory() : DirectoryPath, ReportFileName);
        
        private FileFormat GetFileFormat(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName);

            return fileExtension switch
            {
                ".csv" => FileFormat.CSV,
                ".json" => FileFormat.Json,
                _ => throw new NotSupportedException($"Unsupported file format: {fileExtension}")
            };
        }
    }
}