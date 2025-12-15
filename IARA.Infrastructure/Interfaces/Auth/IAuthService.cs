using IARA.DomainModel.DTOs.Auth;

namespace IARA.Infrastructure.Interfaces.Auth;

public interface IAuthService
{
    Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO request);
    Task<LoginResponseDTO?> RegisterAsync(RegisterRequestDTO request);
    Task<LoginResponseDTO?> RefreshTokenAsync(RefreshTokenRequestDTO request);
    Task<bool> LogoutAsync(int userId);
    Task<bool> ValidateTokenAsync(string token);
}

