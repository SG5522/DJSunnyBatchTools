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
        Task<int> InsertNewIDNumber(CopyParam copyParam);

        /// <summary>
        /// 複製
        /// </summary>
        /// <param name="copyParam"></param>
        Task<int> InsertCopy(CopyParam copyParam);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="copyParam"></param>
        /// <returns></returns>
        Task<int> Update(CopyParam copyParam);
    }
}
