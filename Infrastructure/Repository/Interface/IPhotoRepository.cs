using DBEntities.Entities;
using Infrastructure.Models;

namespace Infrastructure.Repository.Interface
{
    public interface IPhotoRepository
    {
        /// <summary>
        /// 新增人像(SQL)
        /// </summary>
        /// <param name="photo"></param>
        int Insert(Photo photo);

        /// <summary>
        /// 新增人像。 從Idnumber 為key Copy新的人像資料
        /// </summary>
        /// <param name="copyParam"></param>
        /// <returns></returns>
        int InsertCopy(CopyParam copyParam);

        /// <summary>
        /// 更新人像
        /// </summary>
        /// <param name="photo"></param>
        /// <returns></returns>
        int Update(Photo photo);

        /// <summary>
        /// 更新人像的IDNumber (注意此寫法是先新增新的人像在刪除舊人像來處理)
        /// </summary> 
        /// <param name="copyParam"></param>
        /// <returns></returns>
        int UpdateIDNumber(CopyParam copyParam);
    }
}
