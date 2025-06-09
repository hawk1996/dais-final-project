using FinalProject.Repository.Interfaces.BankAccount;
using FinalProject.Repository.Interfaces.User_BankAccount;
using FinalProject.Services.DTOs.BankAccount;
using FinalProject.Services.Helpers;
using FinalProject.Services.Interfaces;

namespace FinalProject.Services.Implementations
{

    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        public BankAccountService(IBankAccountRepository bankAccountRepository)
        {
            _bankAccountRepository = bankAccountRepository;
        }

        public async Task<GetBankAccountResponse> GetByIdAsync(int bankAccountId)
        {
            var bankAccount = await _bankAccountRepository.RetrieveByIdAsync(bankAccountId);
            var bankAccountDto = MappingHelper.MapToDto(bankAccount);
            return new GetBankAccountResponse
            {
                BankAccount = bankAccountDto,
                Success = bankAccountDto != null,
                ErrorMessage = bankAccountDto == null ? "No bank account found with that id" : null
            };
        }

        public async Task<GetBankAccountResponse> GetByNumberAsync(string number)
        {
            var filter = new BankAccountFilter { AccountNumber = number };
            var bankAccount = await _bankAccountRepository.RetrieveCollectionAsync(filter).SingleOrDefaultAsync();
            var bankAccountDto = MappingHelper.MapToDto(bankAccount);
            return new GetBankAccountResponse
            {
                BankAccount = bankAccountDto,
                Success = bankAccountDto != null,
                ErrorMessage = bankAccountDto == null ? "No bank account found with that number" : null
            };
        }
    }
}
