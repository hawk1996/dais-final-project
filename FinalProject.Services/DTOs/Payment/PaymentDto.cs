using FinalProject.Model.Enums;

namespace FinalProject.Services.DTOs.Payment
{
    public class PaymentDto
    {
        public int PaymentId { get; set; }
        public int UserId { get; set; }
        public int FromBankAccountId { get; set; }
        public int ToBankAccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public string Reason;
        public PaymentStatus Status { get; set; }
    }
}
