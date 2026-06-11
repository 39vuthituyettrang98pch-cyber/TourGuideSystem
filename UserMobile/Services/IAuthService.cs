using UserMobile.Models;

namespace UserMobile.Services;

public interface IAuthService
{
    Task<AuthResult> LoginAsync(string email, string password);
    Task<AuthResult> RefreshTokenAsync(string refreshToken);
    Task LogoutAsync();
    Task<bool> IsLoggedInAsync();
}
