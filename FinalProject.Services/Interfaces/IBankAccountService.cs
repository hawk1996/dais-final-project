using FinalProject.Services.DTOs.BankAccount;

namespace FinalProject.Services.Interfaces
{
    public interface IBankAccountService
    {
        Task<GetBankAccountResponse> GetByIdAsync(int bankAccountId);
        Task<GetBankAccountResponse> GetByNumberAsync(string number);
    }
}
