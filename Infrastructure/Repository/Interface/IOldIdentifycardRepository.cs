using DBEntities.Entities;

namespace Infrastructure.Repository.Interface
{
    public interface IOldIdentifycardRepository
    {
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
    }
}
