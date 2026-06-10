using AdminWeb.Data;
using AdminWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdminWeb.Controllers.Api;

[Route("api/[controller]")]
[ApiController]
public class AuthApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthApiController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
        {
            return BadRequest(new { success = false, message = "Email và mật khẩu không được để trống." });
        }

        var exists = await _context.Tourists.AnyAsync(t => t.Email == request.Email);
        if (exists)
        {
            return BadRequest(new { success = false, message = "Email này đã được sử dụng." });
        }

        var tourist = new Tourist
        {
            Email = request.Email,
            FullName = request.FullName,
            PasswordHash = ComputeSha256Hash(request.Password),
            AuthProvider = "local",
            CreatedAt = DateTime.Now
        };

        _context.Tourists.Add(tourist);
        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = "Đăng ký thành công!", data = new { tourist.Id, tourist.Email, tourist.FullName } });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
        {
            return BadRequest(new { success = false, message = "Email và mật khẩu không được để trống." });
        }

        var hashedPassword = ComputeSha256Hash(request.Password);
        var tourist = await _context.Tourists.FirstOrDefaultAsync(t => t.Email == request.Email && t.PasswordHash == hashedPassword);

        if (tourist == null)
        {
            return BadRequest(new { success = false, message = "Email hoặc mật khẩu không chính xác." });
        }

        return Ok(new { success = true, message = "Đăng nhập thành công!", data = new { tourist.Id, tourist.Email, tourist.FullName } });
    }

    private string ComputeSha256Hash(string rawData)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
                builder.Append(bytes[i].ToString("x2"));
            return builder.ToString();
        }
    }
}

public class RegisterRequest
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string FullName { get; set; } = "";
}

public class LoginRequest
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}