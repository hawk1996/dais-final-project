namespace FinalProject.Services.DTOs.Payment
{
    public class CreatePaymentRequest
    {
        public int UserId { get; set; }
        public int FromBankAccountId { get; set; }
        public int ToBankAccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public string Reason;
    }
}
