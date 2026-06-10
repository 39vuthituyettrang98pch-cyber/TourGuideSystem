using AdminWeb.Data;
using AdminWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminWeb.Controllers;

[Authorize]
public class TourTranslationController : Controller
{
    private readonly AppDbContext _context;

    public TourTranslationController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? tourId)
    {
        if (tourId == null) return NotFound();

        var tour = await _context.Tours
            .Include(t => t.Translations)
            .FirstOrDefaultAsync(t => t.Id == tourId);
            
        if (tour == null) return NotFound();

        return View(tour);
    }

    public IActionResult Create(int tourId)
    {
        return View(new TourTranslation { TourId = tourId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TourTranslation translation)
    {
        if (ModelState.IsValid)
        {
            _context.TourTranslations.Add(translation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { tourId = translation.TourId });
        }
        return View(translation);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var translation = await _context.TourTranslations.FindAsync(id);
        if (translation == null) return NotFound();
        return View(translation);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TourTranslation translation)
    {
        if (id != translation.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(translation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { tourId = translation.TourId });
        }
        return View(translation);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var translation = await _context.TourTranslations.FindAsync(id);
        if (translation != null)
        {
            var tourId = translation.TourId;
            _context.TourTranslations.Remove(translation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { tourId = tourId });
        }
        return RedirectToAction("Index", "Tour");
    }
}