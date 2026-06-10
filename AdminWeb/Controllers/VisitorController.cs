using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AdminWeb.Data;
using AdminWeb.Models;
using System.Security.Cryptography;
using System.Text;

namespace AdminWeb.Controllers
{
    public class VisitorController : Controller
    {
        private readonly AppDbContext _context;

        public VisitorController(AppDbContext context)
        {
            _context = context;
        }

        // Bổ sung Lọc (Filter)
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var tourists = from t in _context.Tourists select t;

            if (!string.IsNullOrEmpty(searchString))
            {
                tourists = tourists.Where(t => (t.Email != null && t.Email.Contains(searchString)) || (t.FullName != null && t.FullName.Contains(searchString)));
            }

            return View(await tourists.OrderByDescending(t => t.CreatedAt).ToListAsync());
        }

        // Bổ sung Xem chi tiết
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var tourist = await _context.Tourists
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (tourist == null) return NotFound();

            return View(tourist);
        }

        // GET: Visitor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Visitor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tourist tourist, string Password)
        {
            if (ModelState.IsValid)
            {
                // Mã hóa mật khẩu trước khi lưu vào CSDL
                tourist.PasswordHash = ComputeSha256Hash(Password);

                tourist.CreatedAt = System.DateTime.Now;
                _context.Add(tourist);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm du khách mới thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(tourist);
        }

        // GET: Visitor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var tourist = await _context.Tourists.FindAsync(id);
            if (tourist == null) return NotFound();
            return View(tourist);
        }

        // POST: Visitor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Tourist tourist, string? newPassword)
        {
            if (id != tourist.Id) return NotFound();
            if (ModelState.IsValid)
            {
                var existingTourist = await _context.Tourists.FindAsync(id);
                if (existingTourist == null) return NotFound();

                existingTourist.FullName = tourist.FullName;
                existingTourist.Email = tourist.Email;
                
                if (!string.IsNullOrEmpty(newPassword))
                {
                    // Mã hóa mật khẩu mới
                    existingTourist.PasswordHash = ComputeSha256Hash(newPassword);
                }

                _context.Update(existingTourist);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật thông tin du khách thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(tourist);
        }

        // Bổ sung Xóa
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tourist = await _context.Tourists.FindAsync(id);
            if (tourist != null)
            {
                _context.Tourists.Remove(tourist);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã xóa du khách thành công!";
            }
            return RedirectToAction(nameof(Index));
        }

        // Hàm băm mật khẩu SHA256 (Đồng bộ thuật toán với App Mobile)
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
}