namespace FinalProject.Services.DTOs.Payment
{
    public class CancelPaymentRequest
    {
        public int UserId { get; set; }
        public int PaymentId { get; set; }
    }
}
