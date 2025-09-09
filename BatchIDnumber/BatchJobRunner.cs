using BatchIDnumber.Service.Interface;
using CommonLib.Utils;
using DBEntities.Entities;
using Infrastructure.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace BatchIDnumber
{
    internal class BatchJobRunner : IHostedService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<BatchJobRunner> logger;

        public BatchJobRunner(IServiceProvider serviceProvider, ILogger<BatchJobRunner> logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("批次作業開始執行。");

            // 建立一個 Scope 來處理 Scoped 的服務
            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                var batchService = scope.ServiceProvider.GetRequiredService<IIDNumberBatchService>();
                //List<OrdersView> orders = batchService.GetOrders(new List<string> { "5", "6", "99" });

                //foreach (var order in orders)
                //{
                //    order.RandomCustomerType();
                //}
                //JsonFileUtil.WriteToJsonFile(orders, Path.Combine(Directory.GetCurrentDirectory(), "Report/input.json"));
                //CsvFileUtil.WriteToCsvFile(orders, Path.Combine(Directory.GetCurrentDirectory(), "Report/input.csv"));
                List<OrdersView>? orders = CsvFileUtil.ReadFromCsvFile<OrdersView>(Path.Combine(Directory.GetCurrentDirectory(), "Report/input.csv"));

                if (orders != null)
                {
                    //using (LogContext.PushProperty("EventType", "Input"))
                    //{
                    //    logger.LogInformation($"Total orders: {orders.Count}");

                    //    // 透過迴圈，讓每一筆 Orders 都成為獨立的日誌記錄
                    //    foreach (OrdersView order in orders)
                    //    {
                    //        logger.LogInformation($"{order}");
                    //    }
                    //}

                    // 在這個區塊內，所有日誌都會帶有 EventType = Process 的屬性
                    using (LogContext.PushProperty("EventType", "Process"))
                    {
                        logger.LogInformation("開始處理...");
                        // 假設你的處理過程在這裡
                        batchService.Process(orders);
                        logger.LogInformation("處理完成。");
                    }
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
