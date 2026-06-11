using System.ComponentModel.DataAnnotations;
namespace Backend.DTOs;

public record AuthRequest
{
    [Required(ErrorMessage = "email là bắt buộc")]
    [EmailAddress(ErrorMessage = "email không hợp lệ")]
    public string email { get; init; } = string.Empty;

    [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
    [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
    public string password { get; init; } = string.Empty;
}
