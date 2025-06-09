using FinalProject.Services.DTOs.Payment;

namespace FinalProject.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<CreatePaymentResponse> CreateAsync(CreatePaymentRequest request);
        Task<GetPaymentsByUserResponse> GetPaymentsByUserAsync(int userId);
        Task<ConfirmPaymentResponse> ConfirmAsync(ConfirmPaymentRequest request);
        Task<CancelPaymentResponse> CancelAsync(CancelPaymentRequest request);
    }
}
