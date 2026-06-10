using AdminWeb.Data;
using AdminWeb.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace AdminWeb.Controllers;

[Authorize]
public class PoiController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _hostEnvironment;

    public PoiController(AppDbContext context, IWebHostEnvironment hostEnvironment)
    {
        _context = context;
        _hostEnvironment = hostEnvironment;
    }

    // GET: /Poi/
    public async Task<IActionResult> Index(string? searchString, int page = 1)
    {
        int pageSize = 10; // Giới hạn 10 POI trên 1 trang
        var query = _context.Pois.Include(p => p.Translations).AsQueryable();
        
        // Lọc theo tên POI nếu có nhập từ khóa tìm kiếm
        if (!string.IsNullOrEmpty(searchString))
        {
            var lowerSearch = searchString.ToLower();
            query = query.Where(p => p.Translations!.Any(t => t.Name.ToLower().Contains(lowerSearch)));
        }

        query = query.OrderByDescending(p => p.CreatedAt);

        var totalItems = await query.CountAsync();
        var pois = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        ViewBag.SearchString = searchString; // Truyền từ khóa xuống View để hiển thị lại

        return View(pois);
    }

    // GET: /Poi/Create
    public IActionResult Create()
    {
        return View(new Poi { Latitude = 10.762622m, Longitude = 106.660172m, Radius = 50 });
    }

    // POST: /Poi/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Poi poi, IFormFile? coverImage)
    {
        if (ModelState.IsValid)
        {
            if (coverImage != null && coverImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "poi_covers");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + coverImage.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await coverImage.CopyToAsync(fileStream);
                }
                poi.CoverImageUrl = "/uploads/poi_covers/" + uniqueFileName;
            }

            poi.CreatedAt = DateTime.Now;
            _context.Add(poi);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(poi);
    }

    // GET: /Poi/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var poi = await _context.Pois.FindAsync(id);
        if (poi == null) return NotFound();

        return View(poi);
    }

    // POST: /Poi/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Poi poi, IFormFile? coverImage)
    {
        if (id != poi.Id) return NotFound();

        if (ModelState.IsValid)
        {
            if (coverImage != null && coverImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "poi_covers");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + coverImage.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await coverImage.CopyToAsync(fileStream);
                }
                poi.CoverImageUrl = "/uploads/poi_covers/" + uniqueFileName;
            }

            _context.Update(poi);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(poi);
    }

    // GET: /Poi/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var poi = await _context.Pois.FirstOrDefaultAsync(m => m.Id == id);
        if (poi == null) return NotFound();

        return View(poi);
    }

    // POST: /Poi/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var poi = await _context.Pois.FindAsync(id);
        if (poi != null) _context.Pois.Remove(poi);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: /Poi/ExportExcel
    public async Task<IActionResult> ExportExcel()
    {
        var pois = await _context.Pois
            .Include(p => p.Translations)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        var builder = new StringBuilder();
        builder.Append('\uFEFF'); // Hỗ trợ font tiếng Việt UTF-8 cho Excel
        builder.AppendLine("ID,Ten_Dia_Diem,Vi_Do,Kinh_Do,Ban_Kinh,Trang_Thai,Ngay_Tao");

        foreach (var poi in pois)
        {
            var name = poi.Translations.FirstOrDefault(t => t.LanguageCode == "vi")?.Name ?? $"POI #{poi.Id}";
            name = $"\"{name.Replace("\"", "\"\"")}\""; // Bọc ngoặc kép để tránh lỗi khi tên có chứa dấu phẩy
            builder.AppendLine($"{poi.Id},{name},{poi.Latitude},{poi.Longitude},{poi.Radius},{poi.Status},{poi.CreatedAt:yyyy-MM-dd HH:mm:ss}");
        }

        return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", $"DanhSach_POI_{DateTime.Now:yyyyMMdd_HHmm}.csv");
    }

    // GET: /Poi/CreateTranslation/5
    public IActionResult CreateTranslation(int id)
    {
        var model = new PoiTranslation { PoiId = id };
        return View(model);
    }

    // POST: /Poi/CreateTranslation
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateTranslation(PoiTranslation translation)
    {
        if (ModelState.IsValid)
        {
            var exists = _context.Set<PoiTranslation>().Any(t => t.PoiId == translation.PoiId && t.LanguageCode == translation.LanguageCode);
            if (exists)
            {
                ModelState.AddModelError("LanguageCode", "Bản dịch cho ngôn ngữ này đã tồn tại!");
                return View(translation);
            }

            translation.UpdatedAt = DateTime.Now;
            _context.Add(translation);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "PoiTranslation", new { poiId = translation.PoiId });
        }
        return View(translation);
    }

    // ==========================================
    // QUẢN LÝ DANH MỤC CHO POI (CATEGORIES)
    // ==========================================

    // GET: /Poi/ManageCategories/5
    public async Task<IActionResult> ManageCategories(int id)
    {
        var poi = await _context.Pois
            .Include(p => p.PoiCategories).ThenInclude(pc => pc.Category).ThenInclude(c => c!.Translations)
            .FirstOrDefaultAsync(p => p.Id == id);
            
        if (poi == null) return NotFound();

        var existingCategoryIds = poi.PoiCategories?.Select(pc => pc.CategoryId).ToList() ?? new List<int>();

        ViewBag.AvailableCategories = await _context.Categories
            .Include(c => c.Translations)
            .Where(c => !existingCategoryIds.Contains(c.Id))
            .ToListAsync();

        return View(poi);
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory(int poiId, int categoryId)
    {
        var exists = await _context.PoiCategories.AnyAsync(pc => pc.PoiId == poiId && pc.CategoryId == categoryId);
        if (!exists) {
            _context.PoiCategories.Add(new PoiCategory { PoiId = poiId, CategoryId = categoryId });
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(ManageCategories), new { id = poiId });
    }

    [HttpPost]
    public async Task<IActionResult> RemoveCategory(int poiId, int categoryId)
    {
        var poiCategory = await _context.PoiCategories.FirstOrDefaultAsync(pc => pc.PoiId == poiId && pc.CategoryId == categoryId);
        if (poiCategory != null) {
            _context.PoiCategories.Remove(poiCategory);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(ManageCategories), new { id = poiId });
    }
}