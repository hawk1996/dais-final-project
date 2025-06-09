using System.ComponentModel.DataAnnotations;
using FinalProject.Model.Enums;

namespace FinalProject.Model
{
    public class Payment
    {
        public int PaymentId { get; set; }

        [Required(ErrorMessage = "User is required")]
        public int UserId {  get; set; }

        [Required(ErrorMessage = "Paying account is required")]
        public int FromBankAccountId { get; set; }

        [Required(ErrorMessage = "Account that is getting paid is required")]
        public int ToBankAccountId { get; set; }

        [Required(ErrorMessage = "Transfer amount is required.")]
        [DataType(DataType.Currency)]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be positive.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Timestamp is required")]
        [DataType(DataType.DateTime)]
        public DateTime Timestamp { get; set; }

        [Required(ErrorMessage = "Reason is required")]
        [StringLength(32, ErrorMessage = "Reason can't be longer than 32 characters")]
        public string Reason { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public PaymentStatus Status { get; set; }
    }
}
