using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AdminWeb.Data; 
using AdminWeb.Models;
using System;

namespace AdminWeb.Controllers
{
    public class SyncVersionController : Controller
    {
        private readonly AppDbContext _context;

        public SyncVersionController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.SyncVersions.ToListAsync());
        }

        // Hiển thị Form tạo mới
        public IActionResult Create()
        {
            // Tự động gợi ý mã phiên bản theo ngày giờ hiện tại
            var suggestedVersion = "v" + DateTime.Now.ToString("yyyyMMdd.HHmm");
            return View(new SyncVersion { VersionNumber = suggestedVersion });
        }

        // Xử lý lưu phiên bản mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SyncVersion syncVersion)
        {
            if (ModelState.IsValid)
            {
                syncVersion.CreatedAt = DateTime.Now;
                
                // TODO: Nơi đây sau này bạn có thể gọi hàm để Export Database ra file SQLite hoặc JSON cho App tải về

                _context.SyncVersions.Add(syncVersion);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Đã tạo phiên bản đồng bộ {syncVersion.VersionNumber} thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(syncVersion);
        }

        // Bổ sung Action Delete để xóa phiên bản cũ
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var syncVersion = await _context.SyncVersions.FindAsync(id);
            if (syncVersion == null)
            {
                return NotFound();
            }

            // Có thể thêm logic kiểm tra: không cho phép xóa phiên bản đang Active
            // if (syncVersion.IsActive) { TempData["ErrorMessage"] = "Không thể xóa version đang chạy."; return RedirectToAction(nameof(Index)); }

            _context.SyncVersions.Remove(syncVersion);
            await _context.SaveChangesAsync();
            
            TempData["SuccessMessage"] = "Đã xóa phiên bản đồng bộ thành công.";
            return RedirectToAction(nameof(Index));
        }
    }
}