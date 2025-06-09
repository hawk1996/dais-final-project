namespace FinalProject.Services.DTOs.Payment
{
    public class ConfirmPaymentRequest
    {
        public int UserId { get; set; }
        public int PaymentId { get; set; }
    }
}
