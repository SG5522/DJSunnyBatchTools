using DBEntities.Entities;
using Infrastructure.Models;

namespace Infrastructure.Repository.Interface
{
    public interface IAccTestRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountList"></param>
        /// <returns></returns>
        Task<int> InsertAccTest(AccountRecord accountList);

        List<AccTest> GetAccTest();
    }
}
