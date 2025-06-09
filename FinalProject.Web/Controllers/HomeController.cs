using FinalProject.Services.Interfaces;
using FinalProject.Web.Models.Home;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IUserService _userService;

        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var accounts = await _userService.GetUserBankAccountsAsync(CurrentUserId);

            var model = new HomeViewModel
            {
                BankAccounts = accounts.BankAccounts.Select(a => new BankAccountViewModel
                {
                    BankAccountId = a.BankAccountId,
                    AccountNumber = a.AccountNumber,
                    Balance = a.Balance
                }).ToList()
            };

            return View(model);
        }
    }
}
