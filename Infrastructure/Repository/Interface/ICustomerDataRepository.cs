using DBEntities.Entities;
using Infrastructure.Models;

namespace Infrastructure.Repository.Interface
{
    public interface ICustomerDataRepository
    {
        /// <summary>
        /// 新增一筆Customerdata資料
        /// </summary>
        /// <param name="customerdata"></param>
        /// <returns></returns>
        int Insert(Customerdata customerdata);

        /// <summary>
        /// 依IDNumber複製Customer資料
        /// </summary>
        /// <param name="customerDataParam"></param>
        /// <returns></returns>
        int CopyWithNewId(CopyParam customerDataParam);

        /// <summary>
        /// 依IDNumber 為依據刪除
        /// </summary>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        int Delete(string idNumber);
    }
}
