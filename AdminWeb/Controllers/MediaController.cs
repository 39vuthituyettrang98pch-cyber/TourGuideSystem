using AdminWeb.Data;
using AdminWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminWeb.Controllers;

[Authorize]
public class MediaController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _hostEnvironment;

    public MediaController(AppDbContext context, IWebHostEnvironment hostEnvironment)
    {
        _context = context;
        _hostEnvironment = hostEnvironment;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? poiId)
    {
        if (poiId == null) return NotFound("Thiếu ID của Điểm tham quan");

        var poi = await _context.Pois
            .Include(p => p.Translations)
            .Include(p => p.MediaAssets)
            .FirstOrDefaultAsync(p => p.Id == poiId);

        if (poi == null) return NotFound("Không tìm thấy Điểm tham quan");

        return View(poi);
    }

    [HttpGet]
    public async Task<IActionResult> Library(int? poiId, int page = 1)
    {
        int pageSize = 12;
        var query = _context.MediaAssets
            .Include(m => m.Poi)
            .ThenInclude(p => p!.Translations)
            .AsQueryable();

        if (poiId.HasValue)
        {
            query = query.Where(m => m.PoiId == poiId.Value);
        }

        query = query.OrderByDescending(m => m.Id);

        var totalItems = await query.CountAsync();
        var allMedia = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        ViewBag.Pois = await _context.Pois.Include(p => p.Translations).ToListAsync();
        ViewBag.SelectedPoiId = poiId;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        return View(allMedia);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadMedia(int? poiId, IFormFile? mediaFile, int sortOrder = 0)
    {
        if (poiId == null) return BadRequest("Thiếu POI ID");
        if (mediaFile != null && mediaFile.Length > 0)
        {
            var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "media");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
            
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + mediaFile.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await mediaFile.CopyToAsync(fileStream);
            }
            
            var mediaAsset = new MediaAsset
            {
                PoiId = poiId.Value,
                MediaType = mediaFile.ContentType.StartsWith("video") ? "video" : "image",
                MediaUrl = "/uploads/media/" + uniqueFileName,
                SortOrder = sortOrder
            };
            
            _context.MediaAssets.Add(mediaAsset);
            await _context.SaveChangesAsync();
        }
        
        var referer = Request.Headers["Referer"].ToString();
        if (!string.IsNullOrEmpty(referer) && referer.Contains("Library"))
        {
            return RedirectToAction(nameof(Library), new { poiId = poiId });
        }
        return RedirectToAction(nameof(Index), new { poiId = poiId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteMedia(int id)
    {
        var media = await _context.MediaAssets.FindAsync(id);
        if (media != null)
        {
            var poiId = media.PoiId;
            
            var filePath = Path.Combine(_hostEnvironment.WebRootPath, media.MediaUrl.TrimStart('/'));
            if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);

            _context.MediaAssets.Remove(media);
            await _context.SaveChangesAsync();
            
            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer) && referer.Contains("Library"))
            {
                return RedirectToAction(nameof(Library));
            }
            return RedirectToAction(nameof(Index), new { poiId = poiId });
        }
        return RedirectToAction("Index", "Poi");
    }
}