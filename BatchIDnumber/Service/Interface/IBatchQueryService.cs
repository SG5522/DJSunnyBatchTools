namespace BatchIDnumber.Service.Interface
{
    public interface IBatchQueryService
    {
        Task<string> GetAccountName(string accno);
    }
}
