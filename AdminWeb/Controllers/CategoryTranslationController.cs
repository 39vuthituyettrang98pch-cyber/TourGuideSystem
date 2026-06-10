using AdminWeb.Data;
using AdminWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminWeb.Controllers;

[Authorize]
public class CategoryTranslationController : Controller
{
    private readonly AppDbContext _context;

    public CategoryTranslationController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? categoryId)
    {
        if (categoryId == null) return NotFound();

        var category = await _context.Categories
            .Include(c => c.Translations)
            .FirstOrDefaultAsync(c => c.Id == categoryId);
            
        if (category == null) return NotFound();

        return View(category);
    }

    public IActionResult Create(int categoryId)
    {
        return View(new CategoryTranslation { CategoryId = categoryId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryTranslation translation)
    {
        if (ModelState.IsValid)
        {
            _context.CategoryTranslations.Add(translation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { categoryId = translation.CategoryId });
        }
        return View(translation);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var translation = await _context.CategoryTranslations.FindAsync(id);
        if (translation == null) return NotFound();
        return View(translation);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CategoryTranslation translation)
    {
        if (id != translation.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(translation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { categoryId = translation.CategoryId });
        }
        return View(translation);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var translation = await _context.CategoryTranslations.FindAsync(id);
        if (translation != null)
        {
            var categoryId = translation.CategoryId;
            _context.CategoryTranslations.Remove(translation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { categoryId = categoryId });
        }
        return RedirectToAction("Index", "Category");
    }
}