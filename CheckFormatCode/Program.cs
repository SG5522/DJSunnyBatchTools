using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

class Program
{
    // 嚴格 3-2-9（3位數字-2位數字-9位數字），恰好 16 字元
    private static readonly Regex Format329 = new Regex(@"^\d{3}-\d{2}-\d{9}$", RegexOptions.Compiled);

    static void Main(string[] args)
    {
        string path = "D:\\TEST\\TEST\\ConsoleApp1\\ConsoleApp1\\bin\\Debug\\net6.0\\SAMPLE.TXT";
        if (!File.Exists(path))
        {
            Console.WriteLine($"找不到檔案：{path}");
            return;
        }

        var invalids = new List<string>();
        int total = 0, ok = 0;

        // 自動偵測 BOM（UTF-8/UTF-16）；如需 Big5 可自行指定編碼
        using var sr = new StreamReader(path, detectEncodingFromByteOrderMarks: true);

        string? line;
        while ((line = sr.ReadLine()) != null)
        {
            total++;
            // 位置說明：第5位(1-based) = index 4(0-based)，長度 16 ⇒ [4..19]
            if (line.Length < 20)
            {
                invalids.Add($"第{total}行：長度不足( {line.Length} )，無法擷取第5~20位。原行：{line}");
                continue;
            }

            string segment = line.Substring(4, 16); // 第5~20位
            bool isValid = Format329.IsMatch(segment);

            if (isValid) ok++;
            else invalids.Add($"第{total}行：不合規 → 取到『{segment}』");
        }

        Console.WriteLine($"總行數：{total}，合規：{ok}，不合規：{total - ok}");
        if (invalids.Count > 0)
        {
            Console.WriteLine();
            Console.WriteLine("以下為不合規明細：");
            foreach (var s in invalids) Console.WriteLine(s);
        }
    }
}
