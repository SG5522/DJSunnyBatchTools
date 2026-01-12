using DBEntities.Const;
using Infrastructure.Models;
using System.Text.RegularExpressions;

namespace BatchIDnumber.Util
{
    public partial class TextFileUtil
    {
        /// <summary>
        /// 銀行帳號3-2-9格式
        /// </summary>
        private const string RegexPattern = @"^\d{3}-\d{2}-\d{9}$";

        private const string subCode = @"-00-";

        [GeneratedRegex(RegexPattern, RegexOptions.Compiled)]
        private static partial Regex GetFormat329();

        /// <summary>
        /// 將文字檔的資料轉成List<AccountRecord>
        /// </summary>
        /// <param name="path">路徑</param>
        /// <returns></returns>
        public static List<AccountRecord> FromTextFile(string path)
            => File.ReadLines(path)
                    .Where(lineText => !string.IsNullOrWhiteSpace(lineText))
                    //針對空白做拆分文字陣列
                    .Select(lineText => lineText.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                    .Where(parts => parts.Length >= 4
                                && !parts[1].Contains(subCode)
                                && GetFormat329().IsMatch(parts[1])
                    )
                    .Select(parts => new AccountRecord
                    {
                        AccNo = parts[1],
                        IDNumber = parts[2],
                        CustomerType = (CustomerType)(int)char.GetNumericValue(parts[3][0])
                    }).ToList();        
    }
}
