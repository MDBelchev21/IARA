namespace IARA.Infrastructure.Interfaces.Auth;

public interface ITokenService
{
    string GenerateAccessToken(int userId, string email, string role);
    string GenerateRefreshToken();
    bool ValidateToken(string token);
    int? GetUserIdFromToken(string token);
}

