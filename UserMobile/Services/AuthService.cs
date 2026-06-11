using UserMobile.Models;

namespace UserMobile.Services;

public class AuthService : IAuthService
{
    private const string AccessTokenKey = "access_token";
    private const string RefreshTokenKey = "refresh_token";

    public Task<AuthResult> LoginAsync(string email, string password)
    {
        var token = Guid.NewGuid().ToString();
        var refresh = Guid.NewGuid().ToString();

        Preferences.Set(AccessTokenKey, token);
        Preferences.Set(RefreshTokenKey, refresh);

        return Task.FromResult(new AuthResult
        {
            IsSuccess = true,
            Message = "Login successful.",
            AccessToken = token,
            RefreshToken = refresh,
            Profile = new UserProfile { Email = email, DisplayName = email.Split('@').FirstOrDefault() ?? email, PreferredLanguage = "vi" }
        });
    }

    public Task<AuthResult> RefreshTokenAsync(string refreshToken)
    {
        var accessToken = Guid.NewGuid().ToString();
        Preferences.Set(AccessTokenKey, accessToken);
        return Task.FromResult(new AuthResult { IsSuccess = true, AccessToken = accessToken, RefreshToken = refreshToken });
    }

    public Task LogoutAsync()
    {
        Preferences.Remove(AccessTokenKey);
        Preferences.Remove(RefreshTokenKey);
        return Task.CompletedTask;
    }

    public Task<bool> IsLoggedInAsync()
    {
        var token = Preferences.Get(AccessTokenKey, string.Empty);
        return Task.FromResult(!string.IsNullOrWhiteSpace(token));
    }
}
