using System.ComponentModel.DataAnnotations;

namespace FinalProject.Model
{
    public class BankAccount
    {
        public int BankAccountId { get; set; }

        [Required(ErrorMessage = "Account number is required.")]
        [RegularExpression(@"^[A-Za-z0-9]{22}$", ErrorMessage = "The field must be exactly 22 characters long and contain only letters and numbers.")]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "Balance is required.")]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "Balance must be positive.")]
        public decimal Balance { get; set; }
    }
}
