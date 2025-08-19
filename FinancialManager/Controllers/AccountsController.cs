using FinancialManager.Models;
using FinancialManager.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialManager.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountTypesRepository _accountTypesRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IAccountsRepository _accountRepository;

        public AccountsController(IAccountTypesRepository accountTypesRepository, IUsersRepository usersRepository,
            IAccountsRepository accountRepository)
        {
            _accountTypesRepository = accountTypesRepository;
            _usersRepository = usersRepository;
            _accountRepository = accountRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = _usersRepository.SelectUserId();
            var accounts = await _accountRepository.SelectAccounts(userId);
            var model = accounts
                .GroupBy(x => x.AccountType)
                .Select(x => new IndexAccountViewModel
                {
                    AccountType = x.Key,
                    Accounts = x.AsEnumerable(),
                })
                .ToList();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userId = _usersRepository.SelectUserId();
            var model = new CreateAccountViewModel
            {
                AccountTypes = await GetAccountTypes(userId)
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAccountViewModel account)
        {
            var userId = _usersRepository.SelectUserId();
            var accountType = _accountTypesRepository.SelectAccountType(account.Id, userId);
            if (accountType is null)
            {
                return RedirectToAction("Error", "Home");
            }
            if (!ModelState.IsValid)
            {
                account.AccountTypes = await GetAccountTypes(userId);
                return View(account);
            }
            await _accountRepository.InsertAccount(account);
            return RedirectToAction("Index", "Account");
        }

        private async Task<IEnumerable<SelectListItem>> GetAccountTypes(int userId)
        {
            var accountTypes = await _accountTypesRepository.SelectAccountTypes(userId);
            return accountTypes.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        }
    }
}
