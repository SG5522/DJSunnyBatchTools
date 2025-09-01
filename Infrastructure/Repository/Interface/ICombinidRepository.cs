using DBEntities.Entities;
using Infrastructure.Models;

namespace Infrastructure.Repository.Interface
{
    public interface ICombinidRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="copyParam"></param>
        /// <returns></returns>
        int InsertNewIDNumber(CopyParam copyParam);

        /// <summary>
        /// 複製
        /// </summary>
        /// <param name="copyParam"></param>
        int InsertCopy(CopyParam copyParam);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="copyParam"></param>
        /// <returns></returns>
        public int Update(CopyParam copyParam);
    }
}
