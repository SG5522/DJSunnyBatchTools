using BatchIDnumber.Const;
using BatchIDnumber.Service.Interface;
using CommonLib.Utils;
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


                List<OrdersView>? orders = CsvFileUtil.ReadFromCsvFile<OrdersView>(batchConfigOption.GetAccListPath());

                if (orders != null)
                {
                    logger.LogInformation("開始處理...");                    
                    await batchService.Process(orders);
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
