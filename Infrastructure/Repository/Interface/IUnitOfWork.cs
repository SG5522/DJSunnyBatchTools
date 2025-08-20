namespace Infrastructure.Repository.Interface
{
    /// <summary>
    /// 定義工作單元 (Unit of Work) 的介面。
    /// 工作單元模式確保所有在特定業務操作中的資料庫操作，
    /// 都被視為一個單一的邏輯事務，要麼全部成功提交，要麼全部失敗回滾。
    /// 繼承 IDisposable 以確保資源（如資料庫連線和事務）能被妥善釋放。
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 取得主表 (MainCase) 的 Repository 實例。
        /// 負責處理 MainCase 實體的資料庫操作。
        /// </summary>
        ICustomerDataRepository CustomerDataRepository { get; }

        /// <summary>
        /// 取得印鑑卡歷史紀錄 (OldSealCard) 的 Repository 實例。
        /// 負責處理 OldSealCard 實體的資料庫操作。
        /// </summary>
        ICombinidRepository CombinidRepository { get; }              

        /// <summary>        
        /// 取得印鑑式憑歷史紀錄 (OldSets) 的 Repository 實例。
        /// 負責處理 OldSets 實體的資料庫操作。
        /// </summary>
        IIdentifycardRepository IdentifycardRepository { get; }

        /// <summary>
        /// 取得印鑑歷史紀錄 (OldSeal) 的 Repository 實例。
        /// 負責處理 OldSeal 實體的資料庫操作。
        /// </summary>
        IOldIdentifycardRepository OldIdentifycardRepository { get; }

        /// <summary>
        /// 取得印鑑卡 (SealCard) 的 Repository 實例。
        /// 負責處理 SealCard 實體的資料庫操作。
        /// </summary>
        IOldPhotoRepository OldPhotoRepository { get; }

        /// <summary>
        /// 取得Orders 的 Repository 實例。
        /// 負責處理 Orders 實體的資料庫操作。
        /// </summary>
        IOrdersRepository OrdersRepository { get; }

        /// <summary>
        /// 取得人像 (Photo) 的 Repository 實例。
        /// 負責處理 Photo 實體的資料庫操作。
        /// </summary>
        IPhotoRepository PhotoRepository { get; }

        /// <summary>
        /// 提交所有在這個工作單元中累積的資料庫變更。
        /// 如果所有操作都成功，則將所有處理寫到資料庫中。
        /// </summary>
        void Commit();

        /// <summary>
        /// 回滾所有在這個工作單元中累積的資料庫變更。
        /// 如果任何操作失敗，則撤銷所有未提交的變更，確保資料庫回到操作前的狀態。
        /// </summary>
        void Rollback();
    }
}
