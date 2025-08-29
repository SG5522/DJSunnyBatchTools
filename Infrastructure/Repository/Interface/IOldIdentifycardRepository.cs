using DBEntities.Entities;
using Infrastructure.Models;

namespace Infrastructure.Repository.Interface
{
    public interface IOldIdentifycardRepository
    {
        /// <summary>
        /// 依IdNumber 取得證件歷史紀錄
        /// </summary>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        List<OldIdentifycard> GetOldIdentifycards(string idNumber);

        /// <summary>
        /// 新增多筆證件歷史紀錄(SQL)
        /// </summary>
        /// <param name="identifycard"></param>
        int Inserts(List<OldIdentifycard> oldIdentifycards, string idNumber);

        /// <summary>
        /// 新增證件歷史紀錄(SQL)
        /// </summary>
        /// <param name="oldIdentifycard"></param>
        /// <returns></returns>
        int Insert(OldIdentifycard oldIdentifycard);

        /// <summary>
        /// 依IDNumber更正證件歷史紀錄的IDNumber
        /// </summary>
        /// <param name="copyParam"></param>
        /// <returns></returns>
        int UpdateIDNumber(CopyParam copyParam);
    }
}
