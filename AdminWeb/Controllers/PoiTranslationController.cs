using AdminWeb.Data;
using AdminWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System;

namespace AdminWeb.Controllers;

[Authorize]
public class PoiTranslationController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _hostEnvironment;

    public PoiTranslationController(AppDbContext context, IWebHostEnvironment hostEnvironment)
    {
        _context = context;
        _hostEnvironment = hostEnvironment;
    }

    // Xem danh sách ngôn ngữ của 1 POI
    public async Task<IActionResult> Index(int poiId)
    {
        var poi = await _context.Pois
            .Include(p => p.Translations)
            .FirstOrDefaultAsync(p => p.Id == poiId);

        if (poi == null) return NotFound();

        ViewBag.PoiId = poiId;
        return View(poi.Translations ?? new List<PoiTranslation>());
    }

    // Form thêm ngôn ngữ mới
    public IActionResult Create(int poiId)
    {
        return View(new PoiTranslation { PoiId = poiId });
    }

    // Xử lý lưu ngôn ngữ mới
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PoiTranslation translation, IFormFile? audioFile)
    {
        if (ModelState.IsValid)
        {
            if (audioFile != null && audioFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "audio");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + audioFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await audioFile.CopyToAsync(fileStream);
                }
                translation.AudioUrl = "/uploads/audio/" + uniqueFileName;
            }

            translation.UpdatedAt = DateTime.Now;
            _context.Add(translation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { poiId = translation.PoiId });
        }
        return View(translation);
    }

    // Xem form sửa bản dịch
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var translation = await _context.PoiTranslations.FindAsync(id);
        if (translation == null) return NotFound();
        return View(translation);
    }

    // Xử lý lưu thay đổi bản dịch
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PoiTranslation translation, IFormFile? audioFile)
    {
        if (id != translation.Id) return NotFound();

        if (ModelState.IsValid)
        {
            if (audioFile != null && audioFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "audio");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + audioFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await audioFile.CopyToAsync(fileStream);
                }
                translation.AudioUrl = "/uploads/audio/" + uniqueFileName;
            }

            translation.UpdatedAt = DateTime.Now;
            _context.Update(translation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { poiId = translation.PoiId });
        }
        return View(translation);
    }

    // Xóa bản dịch
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int poiId)
    {
        var translation = await _context.Set<PoiTranslation>().FindAsync(id);
        if (translation != null)
        {
            _context.Remove(translation);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index), new { poiId = poiId });
    }
}