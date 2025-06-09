using FinalProject.Repository.Interfaces.User;
using FinalProject.Services.DTOs.Authentication;
using FinalProject.Services.Helpers;
using FinalProject.Services.Interfaces;

namespace FinalProject.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        public AuthenticationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            var filter = new UserFilter { Username = loginRequest.Username };
            await foreach (var user in _userRepository.RetrieveCollectionAsync(filter))
            {
                var password = SecurityHelper.HashPassword(loginRequest.Password);
                if (string.Equals(user.Password, password, StringComparison.OrdinalIgnoreCase))
                {
                    return new LoginResponse
                    {
                        UserId = user.UserId,
                        Name = user.Name,
                        Success = true
                    };
                }
            }

            return new LoginResponse { ErrorMessage = "Invalid username or password." };
        }
    }
}
