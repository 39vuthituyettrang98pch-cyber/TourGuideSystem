using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _db;
    private readonly IConfiguration _config;

    public AuthService(ApplicationDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public async Task<User?> AuthenticateAsync(AuthRequest request)
    {
        var user = await _db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == request.email);
        if (user == null || !user.IsActive) return null;
        if (!VerifyPassword(request.password, user.PasswordHash)) return null;
        return user;
    }

    public async Task<AuthResponse?> LoginAsync(AuthRequest request)
    {
        var user = await _db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == request.email);
        if (user == null || !user.IsActive) return null;

        // Kiểm tra mật khẩu
        if (!VerifyPassword(request.password, user.PasswordHash)) return null;

        // Tạo access và refresh token
        var accessToken = GenerateAccessToken(user);
        var refreshToken = await CreateRefreshToken(user.Id);

        return new AuthResponse(accessToken.Token, refreshToken.Token, accessToken.ExpiresAt);
    }

    public async Task<bool> LogoutAsync(string? refreshToken)
    {
        if (!string.IsNullOrEmpty(refreshToken))
        {
            var rt = await _db.RefreshTokens.FirstOrDefaultAsync(r => r.Token == refreshToken && !r.IsRevoked);
            if (rt == null) return false;
            rt.IsRevoked = true;
            await _db.SaveChangesAsync();
            return true;
        }

        // No refresh token provided: nothing to revoke here (client may be using cookie).
        return true;
    }

    private (string Token, DateTime ExpiresAt) GenerateAccessToken(User user)
    {
        var jwtSection = _config.GetSection("Jwt");
        var key = jwtSection.GetValue<string>("Key");
        var issuer = jwtSection.GetValue<string>("Issuer");
        var audience = jwtSection.GetValue<string>("Audience");
        var minutes = jwtSection.GetValue<int>("AccessTokenExpirationMinutes");

        var tokenHandler = new JwtSecurityTokenHandler();
        var keyBytes = Encoding.UTF8.GetBytes(key);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role?.Code ?? "user")
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(minutes),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);
        return (tokenString, tokenDescriptor.Expires ?? DateTime.UtcNow.AddMinutes(minutes));
    }

    private async Task<RefreshToken> CreateRefreshToken(Guid userId)
    {
        var jwtSection = _config.GetSection("Jwt");
        var days = jwtSection.GetValue<int>("RefreshTokenExpirationDays");

        var rt = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            ExpiresAt = DateTime.UtcNow.AddDays(days),
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow
        };
        _db.RefreshTokens.Add(rt);
        await _db.SaveChangesAsync();
        return rt;
    }

    private static string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToHexString(hash);
    }

    private static bool VerifyPassword(string provided, string storedHash)
    {
        return string.Equals(HashPassword(provided), storedHash, StringComparison.OrdinalIgnoreCase);
    }
}
