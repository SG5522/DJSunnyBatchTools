using BatchIDnumber.Service.Interface;
using Infrastructure.Repository.Interface;

namespace BatchIDnumber.Service.Implement
{
    public class BatchQueryService : IBatchQueryService
    {
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// 建置
        /// </summary>
        /// <param name="unitOfWork"></param>
        public BatchQueryService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<string> GetAccountName(string accno)
        {
            return await unitOfWork.MainCaseRepository.GetAccName(accno) ?? "主表找不到資料";
        }
    }
}
