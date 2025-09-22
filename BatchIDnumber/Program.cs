using BatchIDnumber;
using BatchIDnumber.Const;
using BatchIDnumber.Service.Implement;
using BatchIDnumber.Service.Interface;
using BatchIDnumber.Util;
using Infrastructure.Repository.Implement;
using Infrastructure.Repository.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

// 在啟動時呼叫 LogUtil 來清理舊日誌檔
LogUtil.CleanOldLogs("logs/input/data-*.json", 10);
LogUtil.CleanOldLogs("logs/results/results-*.json", 10);

//建立主機 (Host)
var builder = Host.CreateDefaultBuilder(args).ConfigureLogging(loggingBuilder =>{loggingBuilder.ClearProviders();})
    // 設定 Serilog
    .UseSerilog((hostContext, services, loggerConfiguration) =>
    {
        loggerConfiguration
            .ReadFrom.Configuration(hostContext.Configuration)
            .Enrich.FromLogContext();
    })
    // 設定依賴注入服務
    .ConfigureServices((hostContext, services) =>
    {
        //批次設定檔載入
        services.Configure<BatchConfigOption>(
            hostContext.Configuration.GetSection("BatchConfig"));

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

// 執行應用程式
await host.RunAsync();
