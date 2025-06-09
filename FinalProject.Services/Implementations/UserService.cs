using FinalProject.Model;
using FinalProject.Repository.Interfaces.BankAccount;
using FinalProject.Repository.Interfaces.User_BankAccount;
using FinalProject.Services.DTOs.BankAccount;
using FinalProject.Services.DTOs.User;
using FinalProject.Services.Helpers;
using FinalProject.Services.Interfaces;

namespace FinalProject.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUser_BankAccountRepository user_BankAccountRepository;
        private readonly IBankAccountRepository bankAccountRepository;
        public UserService(IUser_BankAccountRepository user_BankAccountRepository, IBankAccountRepository bankAccountRepository)
        {
            this.user_BankAccountRepository = user_BankAccountRepository;
            this.bankAccountRepository = bankAccountRepository;
        }

        public async Task<GetUserBankAccountsResponse> GetUserBankAccountsAsync(int userId)
        {
            var bankAccounts = new List<BankAccountDto>();
            var filter = new User_BankAccountFilter { UserId = userId };
            await foreach (var e in user_BankAccountRepository.RetrieveCollectionAsync(filter))
            {
                var bankAccount = await bankAccountRepository.RetrieveByIdAsync(e.BankAccountId);
                if (bankAccount != null)
                    bankAccounts.Add(MappingHelper.MapToDto(bankAccount));
            }

            return new GetUserBankAccountsResponse { BankAccounts = bankAccounts };
        }
    }
}
