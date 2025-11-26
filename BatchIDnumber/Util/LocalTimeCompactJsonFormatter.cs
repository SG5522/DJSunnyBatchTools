using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Json;

namespace BatchIDnumber.Util
{
    //Serilog設定本地時間的處理
    public class LocalTimeJsonFormatter : ITextFormatter
    {
        private readonly JsonValueFormatter valueFormatter;

        // 構造函數
        public LocalTimeJsonFormatter()
        {
            // 使用預設的 JsonValueFormatter 來處理屬性值
            valueFormatter = new JsonValueFormatter();
        }

        public void Format(LogEvent logEvent, TextWriter output)
        {
            // 1. 開始 JSON 物件
            output.Write("{\"@t\":\"");

            // 2. 關鍵：使用當地時間格式化時間戳記
            // 使用 ISO 8601 格式，並在結尾加上 'L' 代表 Local
            output.Write(logEvent.Timestamp.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"));

            output.Write("\",\"@l\":\"");

            // 3. 寫入日誌級別
            output.Write(logEvent.Level.ToString());
            output.Write("\"");

            // 4. 寫入訊息（可選：將渲染後的訊息加入 @m 欄位）
            output.Write(",\"@m\":");
            JsonValueFormatter.WriteQuotedJsonString(logEvent.RenderMessage(), output);

            // 5. 寫入屬性
            foreach (var property in logEvent.Properties)
            {
                output.Write(',');
                // 屬性名稱
                JsonValueFormatter.WriteQuotedJsonString(property.Key, output);
                output.Write(':');
                // 屬性值
                valueFormatter.Format(property.Value, output);
            }

            // 6. 結束 JSON 物件
            output.WriteLine("}");
        }
    }
}