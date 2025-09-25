using BatchIDnumber.Const;
using BatchIDnumber.Service.Implement;
using BatchIDnumber.Service.Interface;
using CommonLib.Utils;
using Infrastructure.Models;
using Infrastructure.Repository.Implement;
using Infrastructure.Repository.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TestApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            // 設定日誌服務
            services.AddLogging(configure => configure.AddConsole());

            // 註冊你的 UnitOfWork 服務
            services.AddScoped<IUnitOfWork>(sp =>
            {
                string? connectionString = "Server=localhost;Database=DJSeal;Integrated Security=SSPI;TrustServerCertificate=True;";

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new InvalidOperationException("Connection string 'DefaultConnection' not found in configuration.");
                }

                var logger = sp.GetRequiredService<ILogger<UnitOfWork>>();
                return new UnitOfWork(connectionString, logger);
            });

            // 註冊你的主要業務服務
            services.AddScoped<IIDNumberBatchService, IDNumberBatchService>();

            // 建立 ServiceProvider
            var serviceProvider = services.BuildServiceProvider();

            // 建立一個 Scope 來處理 Scoped 的服務
            using (var scope = serviceProvider.CreateScope())
            {
                var batchService = scope.ServiceProvider.GetRequiredService<IIDNumberBatchService>();

                // 執行 RamdomCustomerType 類別中的邏輯
                List<AccountRecord> orders = await batchService.GetOrders(new List<string> { "5", "6", "99" });

                foreach (var order in orders)
                {
                    order.RandomCustomerType();
                }


                JsonFileUtil.WriteToJsonFile(orders, Path.Combine(Directory.GetCurrentDirectory(), "test/acclist.json"));
                CsvFileUtil.WriteToCsvFile<AccountRecord, AccountRecordMap>(orders, Path.Combine(Directory.GetCurrentDirectory(), "test/acclist.csv"));
            }

            Console.WriteLine("隨機取得身分別資料已匯出");

        }
    }
}