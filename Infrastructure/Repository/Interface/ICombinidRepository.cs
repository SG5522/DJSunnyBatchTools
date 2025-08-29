using DBEntities.Entities;
using Infrastructure.Models;

namespace Infrastructure.Repository.Interface
{
    public interface ICombinidRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="combinid"></param>
        /// <returns></returns>
        bool Insert(Combinid combinid);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="combinid"></param>
        /// <returns></returns>
        public bool Update(UpdateCombinid combinid);
    }
}
