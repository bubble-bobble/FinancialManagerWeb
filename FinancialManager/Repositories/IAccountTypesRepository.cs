using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialManager.Models;

namespace FinancialManager.Repositories;

public interface IAccountTypesRepository
{
    Task InsertAccountType(AccountTypeViewModel accountType);

    Task<bool> SelectIfExistAccountType(string name, int userId);

    Task<IEnumerable<AccountTypeViewModel>> SelectAccountTypes(int userId);
}