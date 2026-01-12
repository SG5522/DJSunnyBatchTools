using Infrastructure.Models;

namespace BatchIDnumber.Service.Interface
{
    public interface IAccountSupportService
    {
        /// <summary>
        /// 註入test表到AccTest
        /// </summary>
        /// <param name="accountRecords"></param>
        /// <returns></returns>
        Task CreateTestData(List<AccountRecord> accountRecords);

        /// <summary>
        /// test表
        /// </summary>
        /// <returns></returns>
        Task CreateAccTestTxt(string filePath = "AccTestOutput.txt");
    }
}
