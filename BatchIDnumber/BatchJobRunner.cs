using BatchIDnumber.Const;
using BatchIDnumber.Service.Interface;
using BatchIDnumber.Util;
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
        private readonly BatchConfigOption batchConfigOption;

        public BatchJobRunner(IServiceProvider serviceProvider, ILogger<BatchJobRunner> logger, IOptions<BatchConfigOption> options)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
            batchConfigOption = options.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("批次作業開始執行。");

            // 建立一個 Scope 來處理 Scoped 的服務
            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                IIDNumberBatchService batchService = scope.ServiceProvider.GetRequiredService<IIDNumberBatchService>();

                List<AccountRecord>? accountLists = new();
                Console.WriteLine($"開始抓取清單資料 檔名：{batchConfigOption.AccountListFileName}");
                switch (batchConfigOption.FileFormat)
                {
                    case FileFormat.CSV:
                        accountLists = CsvFileUtil.ReadFromCsvFile<AccountRecord>(batchConfigOption.GetAccListPath());
                        break;
                    case FileFormat.Json:
                        accountLists = JsonFileUtil.ReadFromJsonFile<AccountRecord>(batchConfigOption.GetAccListPath());
                        break;
                    case FileFormat.Txt:
                        accountLists = TextFileUtil.FromTextFile(batchConfigOption.GetAccListPath());
                        break;
                    case FileFormat.Unknown:
                        accountLists = null;
                        Console.WriteLine($"無法辯視清單格式");
                        break;
                    default:
                        accountLists = CsvFileUtil.ReadFromCsvFile<AccountRecord>(batchConfigOption.GetAccListPath());
                        break;
                }
                logger.LogInformation("清單資料筆數: {@accountListCount}", accountLists?.Count() ?? 0);                
                Console.WriteLine($"清單資料筆數: {accountLists?.Count() ?? 0}");

                if (accountLists != null)
                {
                    logger.LogInformation("開始處理...");
                    Console.WriteLine("===開始處理===");
                    //過濾帳號只留有籌備處與聯名戶的帳號
                    accountLists = accountLists.Where(accno => accno.CustomerType == CustomerType.Preparation 
                                                            || accno.CustomerType == CustomerType.JointName).ToList();

                    await batchService.Process(accountLists);
                    logger.LogInformation("處理完成。");
                    Console.WriteLine("===處理完成===");
                }
            }

            logger.LogInformation("批次作業結束。");

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
