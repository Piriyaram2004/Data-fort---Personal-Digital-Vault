using PersonalDigitalVault.API.Authentication.DTOs;

namespace PersonalDigitalVault.API.Authentication.Services
{
    public interface IAuthService
    {
        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
        Task ForgotPasswordAsync(ForgotPasswordRequestDto request);
    }
}