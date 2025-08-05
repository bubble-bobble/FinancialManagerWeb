using System.Threading.Tasks;
using FinancialManager.Models;
using FinancialManager.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FinancialManager.Controllers;

public class AccountTypesController : Controller
{
    private readonly IAccountTypesRepository _accountTypesRepository;
    private readonly IUsersRepository _usersRepository;

    public AccountTypesController(IAccountTypesRepository accountTypesRepository, IUsersRepository usersRepository)
    {
        _accountTypesRepository = accountTypesRepository;
        _usersRepository = usersRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = _usersRepository.SelectUserId();
        var accountTypes = await _accountTypesRepository.SelectAccountTypes(userId);
        return View(accountTypes);       
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(AccountTypeViewModel accountType)
    {
        if (!ModelState.IsValid)
        {
            return View(accountType);
        }

        accountType.UserId = _usersRepository.SelectUserId();
        accountType.Sequence = 1;
        await _accountTypesRepository.InsertAccountType(accountType);
        return RedirectToAction("Index", "AccountTypes");
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var userId = _usersRepository.SelectUserId();
        var accountType = await _accountTypesRepository.SelectAccountType(id, userId);
        if (accountType is null)
        {
            return RedirectToAction("Error", "Home");
        }
        return View(accountType);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AccountTypeViewModel accountType)
    {
        var userId = _usersRepository.SelectUserId();
        var accountTypeExist = await _accountTypesRepository.SelectAccountType(accountType.Id, userId);
        if (accountTypeExist is null)
        {
            return RedirectToAction("Error", "Home");
        }
        
        await _accountTypesRepository.UpdateAccountType(accountType);
        return RedirectToAction("Index", "AccountTypes");       
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = _usersRepository.SelectUserId();
        var accountType = await _accountTypesRepository.SelectAccountType(id, userId);
        if (accountType is null)
        {
            return RedirectToAction("Error", "Home");
        }

        return View(accountType);
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmDelete(int id)
    {
        var userId = _usersRepository.SelectUserId();
        var accountType = await _accountTypesRepository.SelectAccountType(id, userId);
        if (accountType is null)
        {
            return RedirectToAction("Error", "Home");
        }

        await _accountTypesRepository.DeleteAccountType(id);
        return RedirectToAction("Index", "AccountTypes");
    }

    public async Task<IActionResult> ValidateAccountTypeName(string name)
    {
        var userId = _usersRepository.SelectUserId();
        var exist = await _accountTypesRepository.SelectIfExistAccountType(name, userId);
        return exist ? Json($"Account type {name} already exists.") : Json(true);
    }
}