bool keepRunning = true;

while (keepRunning)
{
    Console.WriteLine("加密轉換，請輸入功能碼:");
    Console.WriteLine("1. 加密轉換 (明碼 -> 密文)");
    Console.WriteLine("2. 明碼轉回 (密文 -> 明碼)");
    Console.WriteLine("0. 結束程式");

    Console.Write("\n請輸入功能碼: ");

    string? choice = Console.ReadLine();


    switch (choice)
    {
        case "1":
            HandleAction("請輸入明碼: ", input => MyCryptoTool.Encrypt(input));
            break;
        case "2":
            HandleAction("請輸入密文: ", input => MyCryptoTool.Decrypt(input));
            break;
        case "0":
            keepRunning = false;
            Console.WriteLine("程式已結束");
            break;
        default:
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("無效輸入，請輸入 0, 1 或 2。");
            Console.ResetColor();
            break;
    }
}

void HandleAction(string prompt, Func<string, string> cryptoLogic)
{
    Console.Write(prompt);
    string? input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input))
    {
        Console.WriteLine("輸入不可為空！");
        return;
    }

    try
    {
        // 執行你的工具邏輯
        string result = cryptoLogic(input);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"轉換結果: {result}");
        Console.ResetColor();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n錯誤: {ex.Message}");
        Console.ResetColor();
    }
}

// ---------------------------------------------------------
// 你的加密工具 (請將你的工具邏輯放入這裡)
// ---------------------------------------------------------
public static class MyCryptoTool
{    
    public static string Encrypt(string text) => djFileAccess.djED.Encrypt(text);
    
    public static string Decrypt(string cipher) => djFileAccess.djED.Decrypt(cipher);
}
