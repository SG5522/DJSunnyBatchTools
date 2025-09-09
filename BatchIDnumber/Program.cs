using BatchIDnumber;
using BatchIDnumber.Service.Implement;
using BatchIDnumber.Service.Interface;
using Infrastructure.Repository.Implement;
using Infrastructure.Repository.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;

// 這裡就是 .NET 8 的 Top-Level Statements 模式
// 它省略了 static void Main(...) 和 class Program {}

// 1. 建立主機 (Host)
var builder = Host.CreateDefaultBuilder(args).ConfigureLogging(loggingBuilder =>{loggingBuilder.ClearProviders();})
    // 2. 設定 Serilog
    .UseSerilog((hostContext, services, loggerConfiguration) =>
    {
        loggerConfiguration
            .ReadFrom.Configuration(hostContext.Configuration)
            .Enrich.FromLogContext();

        // 透過程式碼動態產生檔名
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string dataPreparation = $"logs/input/data-{timestamp}.json";
        string processingResults = $"logs/results/results-{timestamp}.json";

        // 定義格式器
        CompactJsonFormatter jsonFormatter = new();

        loggerConfiguration.WriteTo.Logger(lc => 
                    lc.Filter.ByIncludingOnly("EventType = 'Input'") // 在程式碼中直接使用篩選
                    .WriteTo.File(
                                    path: dataPreparation,
                                    retainedFileCountLimit: 10,  // 只保留最近 10 個檔案
                                    formatter : jsonFormatter)); // 將篩選過的日誌寫入檔案);

        loggerConfiguration.WriteTo.Logger(lc => 
                lc.Filter.ByExcluding("EventType = 'Process'") // 在程式碼中直接使用篩選
                         .WriteTo.File(
                                    path: processingResults,
                                    retainedFileCountLimit: 10, // 只保留最近 10 個檔案
                                    formatter: jsonFormatter)); // 將篩選過的日誌寫入檔案);
    })
    // 3. 設定依賴注入服務
    .ConfigureServices((hostContext, services) =>
    {
        // 註冊你的 UnitOfWork 服務
        services.AddScoped<IUnitOfWork>(sp =>
        {
            string? connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                // 如果連接字串為 null 或空字串，拋出例外
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found in configuration.");
            }

            var logger = sp.GetRequiredService<ILogger<UnitOfWork>>();
            return new UnitOfWork(connectionString, logger);
        });

        // 註冊你的主要業務服務
        services.AddScoped<IIDNumberBatchService, IDNumberBatchService>();

        // 註冊一個 Hosted Service 來執行你的批次作業
        services.AddHostedService<BatchJobRunner>();
    });

var host = builder.Build();

// 4. 執行應用程式
await host.RunAsync();
