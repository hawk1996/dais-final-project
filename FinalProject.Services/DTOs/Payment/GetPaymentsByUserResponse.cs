namespace FinalProject.Services.DTOs.Payment
{
    public class GetPaymentsByUserResponse : ResponseBase
    {
        public IEnumerable<PaymentDto> Payments { get; set; }
    }
}
