using FinalProject.Services.DTOs.User;

namespace FinalProject.Services.Interfaces
{
    public interface IUserService
    {
        Task<GetUserBankAccountsResponse> GetUserBankAccountsAsync(int userId);
    }
}
