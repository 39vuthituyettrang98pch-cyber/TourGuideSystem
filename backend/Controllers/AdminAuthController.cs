using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Backend.Extensions;

namespace Backend.Controllers;

[ApiController]
[Route("api/admin/auth")]
public class AdminAuthController : ControllerBase
{
    private readonly IAuthService _auth;
    public AdminAuthController(IAuthService auth) => _auth = auth;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthRequest request)
    {
        // Authenticate credentials
        var user = await _auth.AuthenticateAsync(request);
        if (user == null) return Unauthorized(new ApiResponse<object>
        {
            success = false,
            message = "Email hoặc mật khẩu không hợp lệ"
        });

        DebugExtensions.Dump(user);

        // Tạo cookie cho admin
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role?.Code ?? "user")
        };
        var identity = new ClaimsIdentity(claims, "AdminCookie");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("AdminCookie", principal, new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
        });

        return Ok(new ApiResponse<object>
        {
            success = true,
            message = "Đăng nhập thành công",
            data = new { email = user.Email, role = user.Role?.Code }
        });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest? request)
    {
        var ok = await _auth.LogoutAsync(request?.RefreshToken);
        // Sign out cookie for admin sessions as well
        await HttpContext.SignOutAsync("AdminCookie");
        if (!ok) return NotFound();
        return NoContent();
    }

    [Authorize(AuthenticationSchemes = "AdminCookie")]
    [HttpGet("check")]
    public IActionResult Check()
    {
        if (!User.Identity?.IsAuthenticated ?? true) return Unauthorized();
        var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        return Ok(new ApiResponse<object> { success = true, message = "Authenticated", data = new { email, role } });
    }
}
