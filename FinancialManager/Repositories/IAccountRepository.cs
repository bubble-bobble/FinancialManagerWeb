using FinancialManager.Models;
using System.Threading.Tasks;

namespace FinancialManager.Repositories
{
    public interface IAccountRepository
    {
        Task InsertAccount(AccountViewModel account);
    }
}
