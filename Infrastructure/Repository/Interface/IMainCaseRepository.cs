namespace Infrastructure.Repository.Implement
{
    public interface IMainCaseRepository
    {
        /// <summary>
        /// 取得戶名
        /// </summary>
        /// <param name="accNo"></param>
        /// <returns></returns>
        string? GetAccName(string accNo);
    }
}