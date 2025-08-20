using DBEntities.Entities;

namespace Infrastructure.Repository.Interface
{
    public interface IPhotoRepository
    {
        /// <summary>
        /// 新增人像(SQL)
        /// </summary>
        /// <param name="photo"></param>
        int InsertPhoto(Photo photo);
    }
}
