using Backend.DTOs;

namespace Backend.Services;

public interface IAuthService
{
    Task<AuthResponse?> LoginAsync(AuthRequest request);
    Task<bool> LogoutAsync(string? refreshToken);
    Task<Models.User?> AuthenticateAsync(AuthRequest request);
}
