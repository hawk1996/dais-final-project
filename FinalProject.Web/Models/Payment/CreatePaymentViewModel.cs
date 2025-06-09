using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Web.Models.Payment
{
    public class CreatePaymentViewModel
    {
        [Required]
        [Display(Name = "Вашата сметка")]
        public int FromBankAccountId { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> UserBankAccounts { get; set; }

        [Required]
        [Display(Name = "Сметка на получателя")]
        [StringLength(22, MinimumLength = 22, ErrorMessage = "Номера на сметката трябва да е точно 22 символа.")]
        public string ToBankAccountNumber { get; set; }

        [Required]
        [Display(Name = "Сума")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Сумата трябва да бъде над 0.")]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Причина")]
        [StringLength(32, ErrorMessage = "Причината трябва да бъде под 32 символа.")]
        public string Reason { get; set; }
    }
}
