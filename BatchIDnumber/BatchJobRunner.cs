using BatchIDnumber.Const;
using BatchIDnumber.Service.Interface;
using CommonLib.Utils;
using DBEntities.Const;
using Infrastructure.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BatchIDnumber
{
    internal class BatchJobRunner : IHostedService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<BatchJobRunner> logger;
        private BatchConfigOption batchConfigOption;

        public BatchJobRunner(IServiceProvider serviceProvider, ILogger<BatchJobRunner> logger, IOptionsMonitor<BatchConfigOption> optionsMonitor)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
            batchConfigOption = optionsMonitor.CurrentValue;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("批次作業開始執行。");

            // 建立一個 Scope 來處理 Scoped 的服務
            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                var batchService = scope.ServiceProvider.GetRequiredService<IIDNumberBatchService>();

                List<AccountRecord>? accountLists = new();

                switch (batchConfigOption.FileFormat)
                {
                    case FileFormat.CSV:
                        accountLists = CsvFileUtil.ReadFromCsvFile<AccountRecord>(batchConfigOption.GetAccListPath());
                        break;
                    case FileFormat.Json:
                        accountLists = JsonFileUtil.ReadFromJsonFile<AccountRecord>(batchConfigOption.GetAccListPath());
                        break;
                    default:
                        accountLists = CsvFileUtil.ReadFromCsvFile<AccountRecord>(batchConfigOption.GetAccListPath());
                        break;
                }


                if (accountLists != null)
                {
                    logger.LogInformation("開始處理...");
                    //過濾帳號只留有籌備處與聯名戶的帳號
                    accountLists = accountLists.Where(accno => accno.CustomerType == CustomerType.Preparation 
                                                            || accno.CustomerType == CustomerType.JointName).ToList();

                    await batchService.Process(accountLists);
                    logger.LogInformation("處理完成。");                    
                }
            }

            logger.LogInformation("批次作業執行結束。");

            // 為了讓主機自動結束，可以向它發送停止訊號
            IHostApplicationLifetime hostApplicationLifetime = serviceProvider.GetRequiredService<IHostApplicationLifetime>();
            hostApplicationLifetime.StopApplication();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
