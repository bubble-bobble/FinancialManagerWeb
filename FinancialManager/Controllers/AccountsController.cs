using AutoMapper;
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
        private readonly IMapper mapper;

        public AccountsController(IAccountTypesRepository accountTypesRepository, IUsersRepository usersRepository,
            IAccountsRepository accountRepository, IMapper mapper)
        {
            _accountTypesRepository = accountTypesRepository;
            _usersRepository = usersRepository;
            _accountRepository = accountRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int userId = _usersRepository.SelectUserId();
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
            int userId = _usersRepository.SelectUserId();
            var model = new CreateAccountViewModel
            {
                AccountTypes = await GetAccountTypes(userId)
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAccountViewModel account)
        {
            int userId = _usersRepository.SelectUserId();
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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            int userId = _usersRepository.SelectUserId();
            var account = await _accountRepository.SelectAccount(id, userId);
            if (account is null)
            {
                return RedirectToAction("Error", "Home");
            }
            var model = mapper.Map<CreateAccountViewModel>(account);
            model.AccountTypes = await GetAccountTypes(userId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreateAccountViewModel account)
        {
            int userId = _usersRepository.SelectUserId();
            var accountExist = await _accountRepository.SelectAccount(account.Id, userId);
            if (accountExist is null)
            {
                return RedirectToAction("Error", "Home");
            }
            var accountTypeExist = await _accountTypesRepository.SelectAccountType(account.AccountTypeId, userId);
            if (accountTypeExist is null)
            {
                return RedirectToAction("Error", "Home");
            }
            if (!ModelState.IsValid)
            {
                account.AccountTypes = await GetAccountTypes(userId);
                return View(account);
            }
            await _accountRepository.UpdateAccount(account);
            return RedirectToAction("Index", "Accounts");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            int userId = _usersRepository.SelectUserId();
            var account = await _accountRepository.SelectAccount(id, userId);
            if (account is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(account);
        }

        public async Task<IActionResult> ConfirmDelete(int id)
        {
            int userId = _usersRepository.SelectUserId();
            var account = await _accountRepository.SelectAccount(id, userId);
            if (account is null)
            {
                return RedirectToAction("Error", "Home");
            }
            await _accountRepository.DeleteAccount(id);
            return RedirectToAction("Index", "Accounts");
        }

        private async Task<IEnumerable<SelectListItem>> GetAccountTypes(int userId)
        {
            var accountTypes = await _accountTypesRepository.SelectAccountTypes(userId);
            return accountTypes.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        }
    }
}
