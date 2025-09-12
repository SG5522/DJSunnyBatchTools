namespace BatchIDnumber.Util
{    
    public class LogUtil
    {
        /// <summary>
        /// 自動清理符合指定模式的舊日誌檔案，只保留最新的 N 個。
        /// </summary>
        /// <param name="pathPattern">日誌檔案的完整路徑模式，例如：logs/input/data-*.json</param>
        /// <param name="countToRetain">要保留的最新檔案數量。</param>
        public static void CleanOldLogs(string pathPattern, int countToRetain)
        {
            // 檢查資料夾是否存在
            string? directoryPath = Path.GetDirectoryName(pathPattern);
            if (string.IsNullOrWhiteSpace(directoryPath) || !Directory.Exists(directoryPath))
            {
                return;
            }

            // 取得所有符合指定路徑模式的檔案
            var filesToClean = Directory.GetFiles(directoryPath, Path.GetFileName(pathPattern))
                                        .Select(f => new FileInfo(f))
                                        .OrderByDescending(f => f.CreationTime)
                                        .Skip(countToRetain);

            // 刪除其餘的舊檔案
            foreach (var file in filesToClean)
            {
                try
                {
                    File.Delete(file.FullName);
                    Console.WriteLine($"已刪除舊日誌檔：{file.FullName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"刪除日誌檔失敗 {file.FullName}：{ex.Message}");
                }
            }
        }
    }
}
