using DBEntities.Entities;
using Infrastructure.Models;

namespace Infrastructure.Repository.Interface
{
    public interface IOldPhotoRepository
    {
        /// <summary>
        /// 依IdNumber 取得人像歷史紀錄
        /// </summary>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        Task<List<OldPhoto>> GetOldPhotos(string idNumber);

        /// <summary>
        /// 新增多筆人像歷史紀錄(SQL)
        /// </summary>
        /// <param name="oldPhotos"></param>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        Task<int> Inserts(List<OldPhoto> oldPhotos, string idNumber);

        /// <summary>
        /// 新增人像歷史紀錄(SQL)
        /// </summary>
        /// <param name="oldPhoto"></param>
        /// <returns></returns>
        Task<int> Insert(OldPhoto oldPhoto);

        /// <summary>
        /// 更新人像歷史紀錄的IDNumber
        /// </summary>
        /// <param name="copyParam"></param>
        /// <returns></returns>
        Task<int> UpdateIDNumber(CopyParam copyParam);
    }
}
