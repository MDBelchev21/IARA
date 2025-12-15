using IARA.DomainModel.DTOs.Auth;
using IARA.Infrastructure.Interfaces;
using IARA.Infrastructure.Interfaces.Auth;
using IARA.Persistance.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IARA.BusinessLogic.Services.Auth;

public class AuthService : BaseService, IAuthService
{
    private readonly ITokenService _tokenService;

    public AuthService(BaseServiceInjector injector, ITokenService tokenService) : base(injector)
    {
        _tokenService = tokenService;
    }

    public async Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO request)
    {
        var person = await Db.Persons
            .FirstOrDefaultAsync(p => p.Email == request.Email);

        if (person == null)
            return null;

        if (!VerifyPassword(request.Password, person.PasswordHash))
            return null;

        string role = await DetermineUserRole(person.PersonId);

        var accessToken = _tokenService.GenerateAccessToken(person.PersonId, person.Email, role);
        var refreshToken = _tokenService.GenerateRefreshToken();

        person.RefreshToken = refreshToken;
        person.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await Db.SaveChangesAsync();

        return new LoginResponseDTO
        {
            Token = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            UserName = $"{person.FirstName} {person.LastName}",
            Email = person.Email,
            Role = role,
            UserId = person.PersonId
        };
    }

    public async Task<LoginResponseDTO?> RegisterAsync(RegisterRequestDTO request)
    {
        var existingPerson = await Db.Persons
            .FirstOrDefaultAsync(p => p.Email == request.Email || p.EGN == request.EGN);

        if (existingPerson != null)
            return null;

        var person = new Person
        {
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            EGN = request.EGN,
            Address = request.Address,
            PasswordHash = HashPassword(request.Password),
            CreatedOn = DateTime.UtcNow
        };

        Db.Persons.Add(person);
        await Db.SaveChangesAsync();

        var accessToken = _tokenService.GenerateAccessToken(person.PersonId, person.Email, "User");
        var refreshToken = _tokenService.GenerateRefreshToken();

        person.RefreshToken = refreshToken;
        person.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await Db.SaveChangesAsync();

        return new LoginResponseDTO
        {
            Token = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            UserName = $"{person.FirstName} {person.LastName}",
            Email = person.Email,
            Role = "User",
            UserId = person.PersonId
        };
    }

    public async Task<LoginResponseDTO?> RefreshTokenAsync(RefreshTokenRequestDTO request)
    {
        var userId = _tokenService.GetUserIdFromToken(request.Token);
        if (userId == null)
            return null;

        var person = await Db.Persons.FindAsync(userId.Value);
        if (person == null || person.RefreshToken != request.RefreshToken)
            return null;

        if (person.RefreshTokenExpiryTime <= DateTime.UtcNow)
            return null;

        string role = await DetermineUserRole(person.PersonId);

        var accessToken = _tokenService.GenerateAccessToken(person.PersonId, person.Email, role);
        var refreshToken = _tokenService.GenerateRefreshToken();

        person.RefreshToken = refreshToken;
        person.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await Db.SaveChangesAsync();

        return new LoginResponseDTO
        {
            Token = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            UserName = $"{person.FirstName} {person.LastName}",
            Email = person.Email,
            Role = role,
            UserId = person.PersonId
        };
    }

    public async Task<bool> LogoutAsync(int userId)
    {
        var person = await Db.Persons.FindAsync(userId);
        if (person == null)
            return false;

        person.RefreshToken = null;
        person.RefreshTokenExpiryTime = null;
        await Db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        return _tokenService.ValidateToken(token);
    }

    private async Task<string> DetermineUserRole(int personId)
    {
        var isAdmin = await Db.Administrators.AnyAsync(a => a.PersonId == personId);
        if (isAdmin)
            return "Administrator";

        var isInspector = await Db.Inspectors.AnyAsync(i => i.PersonId == personId);
        if (isInspector)
            return "Inspector";

        var isShipOwner = await Db.ShipOwners.AnyAsync(so => so.PersonId == personId && so.IsActive);
        if (isShipOwner)
            return "ShipOwner";

        var isRecFisherman = await Db.RecreationalFishermen.AnyAsync(rf => rf.PersonId == personId);
        if (isRecFisherman)
            return "RecreationalFisherman";

        return "User";
    }

    private string HashPassword(string password)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    private bool VerifyPassword(string password, string? storedHash)
    {
        if (string.IsNullOrEmpty(storedHash))
            return false;

        var hash = HashPassword(password);
        return hash == storedHash;
    }
}

