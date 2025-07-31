using System.Threading.Tasks;
using FinancialManager.Models;
using FinancialManager.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FinancialManager.Controllers;

public class AccountTypesController : Controller
{
    private readonly IAccountTypesRepository _accountTypesRepository;

    public AccountTypesController(IAccountTypesRepository accountTypesRepository)
    {
        _accountTypesRepository = accountTypesRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        const int userId = 1;
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

        accountType.UserId = 0;
        accountType.UserId = 1;
        await _accountTypesRepository.InsertAccountType(accountType);
        return RedirectToAction("Index", "AccountTypes");
    }

    public async Task<IActionResult> ValidateAccountTypeName(string name)
    {
        var exist = await _accountTypesRepository.SelectIfExistAccountType(name, 1);
        return exist ? Json($"Account type {name} already exists.") : Json(true);
    }
}