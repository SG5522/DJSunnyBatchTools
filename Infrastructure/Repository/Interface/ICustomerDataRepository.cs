using DBEntities.Entities;

namespace Infrastructure.Repository.Interface
{
    public interface ICustomerDataRepository
    {

        int Insert(Customerdata customerdata);

        int InsertWithOldIdnumber(string idNumber, int count);
    }
}
