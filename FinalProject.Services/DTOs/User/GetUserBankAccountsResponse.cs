using FinalProject.Services.DTOs.BankAccount;

namespace FinalProject.Services.DTOs.User
{
    public class GetUserBankAccountsResponse
    {
        public IEnumerable<BankAccountDto> BankAccounts { get; set; }
    }
}
