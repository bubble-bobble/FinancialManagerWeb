using FinancialManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialManager.Repositories
{
    public interface IAccountsRepository
    {
        Task InsertAccount(AccountViewModel account);
        Task<IEnumerable<AccountViewModel>> SelectAccounts(int userId);
    }
}
