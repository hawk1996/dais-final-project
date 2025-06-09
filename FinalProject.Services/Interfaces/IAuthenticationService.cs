using FinalProject.Services.DTOs.Authentication;

namespace FinalProject.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
    }
}
