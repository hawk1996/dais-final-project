using FinalProject.Services.DTOs.Payment;
using FinalProject.Services.Interfaces;
using FinalProject.Web.Models.Payment;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using FinalProject.Web.Models.Enums;

namespace FinalProject.Web.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IPaymentService _paymentService;
        private readonly IBankAccountService _bankAccountService;

        public PaymentController(
            IUserService userService,
            IPaymentService paymentService,
            IBankAccountService bankAccountService)
        {
            _userService = userService;
            _paymentService = paymentService;
            _bankAccountService = bankAccountService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userBankAccounts = await _userService.GetUserBankAccountsAsync(CurrentUserId);
            return await RenderCreatePaymentView(new CreatePaymentViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePaymentViewModel model)
        {
            if (!ModelState.IsValid)
                return await RenderCreatePaymentView(model);

            var toBankAccount = await _bankAccountService.GetByNumberAsync(model.ToBankAccountNumber);

            if (toBankAccount.BankAccount == null)
            {
                TempData["ErrorMessage"] = toBankAccount.ErrorMessage;
                return await RenderCreatePaymentView(model);
            }

            var request = new CreatePaymentRequest
            {
                UserId = CurrentUserId,
                FromBankAccountId = model.FromBankAccountId,
                ToBankAccountId = toBankAccount.BankAccount.BankAccountId,
                Amount = model.Amount,
                Reason = model.Reason,
                Timestamp = DateTime.UtcNow
            };

            var response = await _paymentService.CreateAsync(request);

            if (!response.Success)
            {
                TempData["ErrorMessage"] = response.ErrorMessage ?? "Failed to create payment.";
                return await RenderCreatePaymentView(model);
            }

            return RedirectToAction("MyPayments", "Payment");
        }

        [HttpGet]
        public async Task<IActionResult> MyPayments(PaymentSortOption sort = PaymentSortOption.ByDateDesc)
        {
            var payments = await _paymentService.GetPaymentsByUserAsync(CurrentUserId);
            var paymentViewModelTasks = payments.Payments.Select(async p =>
            {
                var fromAccount = await _bankAccountService.GetByIdAsync(p.FromBankAccountId);
                var toAccount = await _bankAccountService.GetByIdAsync(p.ToBankAccountId);

                return new PaymentViewModel
                {
                    PaymentId = p.PaymentId,
                    FromAccount = fromAccount.BankAccount.AccountNumber,
                    ToAccount = toAccount.BankAccount.AccountNumber,
                    Amount = p.Amount,
                    Reason = p.Reason,
                    Timestamp = p.Timestamp,
                    Status = TranslateStatus(p.Status.ToString())
                };
            });

            var model = await Task.WhenAll(paymentViewModelTasks);

            model = sort switch
            {
                PaymentSortOption.ByStatus => model.OrderBy(p => p.Status).ToArray(),
                _ => model.OrderByDescending(p => p.Timestamp).ToArray(), // default: newest first
            };

            ViewBag.CurrentSort = sort;
            return View(model.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmPayment(int paymentId)
        {
            var response = await _paymentService.ConfirmAsync(new ConfirmPaymentRequest { PaymentId = paymentId, UserId = CurrentUserId });
            if (!response.Success)
            {
                TempData["ErrorMessage"] = response.ErrorMessage;
            }

            return RedirectToAction(nameof(MyPayments));
        }

        [HttpPost]
        public async Task<IActionResult> CancelPayment(int paymentId)
        {
            var response = await _paymentService.CancelAsync(new CancelPaymentRequest { PaymentId = paymentId, UserId = CurrentUserId });
            if (!response.Success)
            {
                TempData["ErrorMessage"] = response.ErrorMessage;
            }

            return RedirectToAction(nameof(MyPayments));
        }

        private async Task<IActionResult> RenderCreatePaymentView(CreatePaymentViewModel model)
        {
            var userBankAccounts = await _userService.GetUserBankAccountsAsync(CurrentUserId);
            model.UserBankAccounts = userBankAccounts.BankAccounts.Select(ba => new SelectListItem
            {
                Value = ba.BankAccountId.ToString(),
                Text = $"{ba.AccountNumber} - {ba.Balance:C}"
            });
            
            return View(model);
        }

        private static string TranslateStatus(string status)
        {
            return status switch
            {
                "Pending" => "ИЗЧАКВА",
                "Processed" => "ОБРАБОТЕН",
                "Cancelled" => "ОТКАЗАН",
                _ => status
            };
        }

    }
}
