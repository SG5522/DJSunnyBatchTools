using DBEntities.Const;
using Infrastructure.Models;
using System.Text.RegularExpressions;

namespace BatchIDnumber.Util
{
    public class TextFileUtil
    {
        /// <summary>
        /// 銀行帳號3-2-9格式
        /// </summary>
        private const string RegexPattern = @"^\d{3}-\d{2}-\d{9}$";

        private const string subCode = @"-00-";

        private static readonly Regex Format329 = new(RegexPattern, RegexOptions.Compiled);

        /// <summary>
        /// 將文字檔的資料轉成List<AccountRecord>
        /// </summary>
        /// <param name="path">路徑</param>
        /// <returns></returns>
        public static List<AccountRecord> FromTextFile (string path)
            => File.ReadAllLines(path)
                    .Where(lineText => !string.IsNullOrWhiteSpace(lineText)
                            && lineText.Length >= 35
                            && lineText.Substring(7, 4) != subCode
                            && Format329.IsMatch(lineText.Substring(4, 16).Trim()))
                    .Select(LineTextToClass)
                    .ToList();

        /// <summary>
        /// 針對每行Text做class轉換
        /// </summary>
        /// <param name="lineText"></param>
        /// <returns></returns>
        private static AccountRecord LineTextToClass(string lineText)
            =>  new(){
                        AccNo = lineText.Substring(4, 16).Trim(),
                        IDNumber = lineText.Substring(21, 10).Trim(),
                        CustomerType = (CustomerType)(int)char.GetNumericValue(lineText[^1]) //[^1] 指倒數最後一個
                };
        
    }
}
