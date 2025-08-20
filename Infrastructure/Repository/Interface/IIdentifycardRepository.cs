using DBEntities.Entities;

namespace Infrastructure.Repository.Interface
{
    public interface IIdentifycardRepository
    {
        /// <summary>
        /// 新增證件(SQL)
        /// </summary>
        /// <param name="identifycard"></param>
        int Insert(Identifycard identifycard);
    }
}
