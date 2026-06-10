using AdminWeb.Data;
using AdminWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace AdminWeb.Controllers;

[Authorize(Roles = "Admin")] // Chỉ tài khoản có Role "Admin" mới được phép vào
public class UserController : Controller
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _context.Users.Include(u => u.Role).ToListAsync();
        return View(users);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Roles = await _context.Roles.ToListAsync();
        return View(new User());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(User user)
    {
        if (ModelState.IsValid)
        {
            user.CreatedAt = DateTime.Now;
            user.PasswordHash = ComputeSha256Hash(user.PasswordHash);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Roles = await _context.Roles.ToListAsync();
        return View(user);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();
        
        ViewBag.Roles = await _context.Roles.ToListAsync();
        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, User user, string? newPassword)
    {
        if (id != user.Id) return NotFound();

        if (ModelState.IsValid)
        {
            var existingUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            if (existingUser == null) return NotFound();

            if (!string.IsNullOrEmpty(newPassword))
            {
                user.PasswordHash = ComputeSha256Hash(newPassword);
            }
            else
            {
                user.PasswordHash = existingUser.PasswordHash;
            }
            
            user.CreatedAt = existingUser.CreatedAt;

            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Roles = await _context.Roles.ToListAsync();
        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private string ComputeSha256Hash(string rawData)
    {
        if (string.IsNullOrEmpty(rawData)) return "";
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