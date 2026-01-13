using BatchIDnumber.Service.Interface;
using Infrastructure.Models;
using Infrastructure.Repository.Interface;
using Microsoft.Extensions.Logging;
using System.Text;

namespace BatchIDnumber.Service.Implement
{
    /// <summary>
    /// 建立測試帳號用的處理
    /// </summary>    
    /// <param name="unitOfWork"></param>    
    public class AccountSupportService(IUnitOfWork unitOfWork) : IAccountSupportService
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;        

        // 將資料寫入測試文字檔
        public async Task CreateAccTestTxt(string filePath = "AccTestOutput.txt")
        {
            var accTests = unitOfWork.AccTestRepository.GetAccTest();
            StringBuilder sb = new();

            foreach (var accTest in accTests)
            {
                // 利用 C# 8.0+ 的範圍運算子 [..3] 取前三個字，更簡潔
                string firstSegment = accTest.AccNo.Length >= 3 ? accTest.AccNo[..3] : string.Empty;
            
                string line = $"{firstSegment} {accTest.AccNo} {accTest.IDNumber.PadRight(15)} {(int)accTest.CustomerType}";
                sb.AppendLine(line);
            }

            await File.WriteAllTextAsync(filePath, sb.ToString(), Encoding.UTF8);
        }

        // 插入測試資料
        public async Task CreateTestData(List<AccountRecord> accountRecords)
        {
            foreach (var accountRecord in accountRecords)
            {
                await unitOfWork.AccTestRepository.InsertAccTest(accountRecord);
            }
            unitOfWork.Commit();
        }
    }
}