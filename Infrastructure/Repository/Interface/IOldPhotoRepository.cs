using DBEntities.Entities;

namespace Infrastructure.Repository.Interface
{
    public interface IOldPhotoRepository
    {
        /// <summary>
        /// 新增多筆人像歷史紀錄(SQL)
        /// </summary>
        /// <param name="oldPhotos"></param>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        int Inserts(List<OldPhoto> oldPhotos, string idNumber);

        /// <summary>
        /// 新增人像歷史紀錄(SQL)
        /// </summary>
        /// <param name="oldPhoto"></param>
        /// <returns></returns>
        int Insert(OldPhoto oldPhoto);
    }
}
