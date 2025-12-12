using Infrastructure.Repository.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Infrastructure.Repository.Implement
{
    /// 定義工作單元 (Unit of Work) 的介面。
    /// 工作單元模式確保所有在特定業務操作中的資料庫操作，
    /// 都被視為一個單一的邏輯事務，要麼全部成功提交，要麼全部失敗回滾。
    /// 繼承 IDisposable 以確保資源（如資料庫連線和事務）能被妥善釋放。
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection connection;
        private readonly IDbTransaction transaction;
        private readonly ILogger<UnitOfWork> logger;

        // IDisposable 模式所需欄位，用於追蹤是否已處置資源。
        private bool disposedValue = false;

        // 私有欄位來儲存 Repository 實例，實作延遲載入
        private readonly ICustomerDataRepository customerDataRepository;
        private readonly ICombinidRepository combinidRepository;
        private readonly IIdentifycardRepository identifycardRepository;
        private readonly IOldIdentifycardRepository oldIdentifycardRepository;
        private readonly IOldPhotoRepository oldPhotoRepository;
        private readonly IOrdersRepository ordersRepository;
        private readonly IPhotoRepository photoRepository;
        private readonly IMainCaseRepository mainCaseRepository;
        private readonly IAccTestRepository accTestRepository;

        /// <summary>
        /// 建置
        /// </summary>
        /// <param name="connectionString"></param>
        public UnitOfWork(string connectionString, ILogger<UnitOfWork> logger)
        {
            connection = new SqlConnection(connectionString); // 或 SQLiteConnection, OracleConnection 等
            connection.Open();
            transaction = connection.BeginTransaction();

            // 在這裡初始化所有 Repository 實例
            customerDataRepository = new CustomerDataRepository(connection, transaction);
            combinidRepository = new CombinidRepository(connection, transaction);
            identifycardRepository = new IdentifycardRepository(connection, transaction);
            oldIdentifycardRepository = new OldIdentifycardRepository(connection, transaction);
            oldPhotoRepository = new OldPhotoRepository(connection, transaction);
            ordersRepository = new OrdersRepository(connection, transaction);
            photoRepository = new PhotoRepository(connection, transaction);
            mainCaseRepository = new MainCaseRepository(connection, transaction);
            accTestRepository = new AccTestRepository(connection, transaction);
            this.logger = logger;
        }

        /// <inheritdoc/>
        public ICustomerDataRepository CustomerDataRepository => customerDataRepository;

        /// <inheritdoc/>
        public ICombinidRepository CombinidRepository => combinidRepository;

        /// <inheritdoc/>
        public IIdentifycardRepository IdentifycardRepository => identifycardRepository;
               
        /// <inheritdoc/>
        public IOldIdentifycardRepository OldIdentifycardRepository => oldIdentifycardRepository;

        /// <inheritdoc/>
        public IOldPhotoRepository OldPhotoRepository => oldPhotoRepository;

        /// <inheritdoc/>
        public IOrdersRepository OrdersRepository => ordersRepository;

        /// <inheritdoc/>
        public IPhotoRepository PhotoRepository => photoRepository;

        public IMainCaseRepository MainCaseRepository => mainCaseRepository;

        public IAccTestRepository AccTestRepository => accTestRepository;


        /// <inheritdoc/>
        public void Commit()
        {
            try
            {
                // 執行事務提交
                transaction.Commit();
            }
            catch
            {
                // 如果提交失敗，則回滾事務
                transaction.Rollback();
                // 重新拋出異常，讓上層呼叫者能夠捕獲並處理
                throw;
            }
            finally
            {
                // 無論事務是否提交成功，都呼叫 Dispose 釋放資源
                Dispose();
            }
        }

        /// <inheritdoc/>
        public void Rollback()
        {
            try
            {
                // 執行事務回滾
                transaction.Rollback();
            }
            catch (Exception ex)
            {
                // 記錄回滾失敗的錯誤，但不再拋出，因為已經是錯誤處理流程
                logger.LogError($"Error during transaction rollback: {ex.Message}");
            }
            finally
            {
                // 無論事務是否回滾成功，都呼叫 Dispose 釋放資源
                Dispose();
            }
        }


        /// <summary>
        /// 實現 IDisposable 介面，用於釋放資源。
        /// 這個方法是 Dispose 模式的核心。
        /// </summary>
        /// <param name="disposing">表示是否正在處置受管理資源。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue) // 檢查是否已經處置過
            {
                if (disposing) // 如果是從 Dispose() 呼叫 (正在處置受管理資源)
                {
                    // 處置資料庫事務，確保它被關閉或釋放
                    if (transaction != null)
                    {
                        transaction.Dispose();
                        // transaction = null; // 可選：將引用設為 null，幫助 GC 更早回收
                    }
                    // 處置資料庫連線，確保它被關閉並釋放
                    if (connection != null && connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                        connection.Dispose();                        
                    }
                }

                disposedValue = true; // 標記為已處置
            }
        }

        /// <summary>
        /// 公開的 Dispose 方法，允許外部呼叫者顯式釋放資源。
        /// 這是 IDisposable 介面的入口點。
        /// </summary>
        public void Dispose()
        {
            // 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法中。
            Dispose(disposing: true); // 呼叫 Dispose 方法，指示處置受管理資源
            // 通知垃圾回收器此物件已被手動處置，不需要再次呼叫完成項
            GC.SuppressFinalize(this);
        }
    }
}
