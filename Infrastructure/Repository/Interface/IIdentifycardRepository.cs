using DBEntities.Entities;
using Infrastructure.Models;

namespace Infrastructure.Repository.Interface
{
    public interface IIdentifycardRepository
    {
        /// <summary>
        /// 新增證件(SQL)
        /// </summary>
        /// <param name="identifycard"></param>
        Task<int> Insert(IdentifyCard identifycard);

        /// <summary>
        /// 依Idunumber為Key 複製證件
        /// </summary>
        /// <param name="copyParam"></param>
        /// <returns></returns>
        Task<int> InsertCopy(CopyParam copyParam);

        /// <summary>
        /// 依Idnumber 找出該筆資料(可能證件正反面) 更新新的IDNumber
        /// </summary>
        /// <param name="copyParam"></param>
        /// <returns></returns>
        Task<int> UpdateIDNumber(CopyParam copyParam);
    }
}
