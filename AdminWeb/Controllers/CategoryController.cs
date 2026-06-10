using AdminWeb.Data;
using AdminWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminWeb.Controllers;

[Authorize]
public class CategoryController : Controller
{
    private readonly AppDbContext _context;

    public CategoryController(AppDbContext context)
    {
        _context = context;
    }

    // Danh sách Danh mục
    public async Task<IActionResult> Index()
    {
        var categories = await _context.Categories
            .Include(c => c.Translations)
            .ToListAsync();
        return View(categories);
    }

    public IActionResult Create()
    {
        return View(new Category());
    }

    // Tạo mới Danh mục (tự động thêm tên Tiếng Việt)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category category, string Name)
    {
        if (ModelState.IsValid)
        {
            _context.Add(category);
            await _context.SaveChangesAsync();

            if (!string.IsNullOrEmpty(Name))
            {
                _context.CategoryTranslations.Add(new CategoryTranslation
                {
                    CategoryId = category.Id,
                    LanguageCode = "vi",
                    Name = Name
                });
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        return View(category);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound();
        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Category category)
    {
        if (id != category.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category != null) _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // ==========================================
    // QUẢN LÝ BẢN DỊCH DANH MỤC
    // ==========================================
    public async Task<IActionResult> Translations(int? id)
    {
        if (id == null) return NotFound();

        var category = await _context.Categories
            .Include(c => c.Translations)
            .FirstOrDefaultAsync(c => c.Id == id);
            
        if (category == null) return NotFound();

        return View(category);
    }

    public IActionResult CreateTranslation(int categoryId)
    {
        return View(new CategoryTranslation { CategoryId = categoryId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateTranslation(CategoryTranslation translation)
    {
        if (ModelState.IsValid)
        {
            _context.CategoryTranslations.Add(translation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Translations), new { id = translation.CategoryId });
        }
        return View(translation);
    }

    public async Task<IActionResult> EditTranslation(int? id)
    {
        if (id == null) return NotFound();
        var translation = await _context.CategoryTranslations.FindAsync(id);
        if (translation == null) return NotFound();
        return View(translation);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditTranslation(int id, CategoryTranslation translation)
    {
        if (id != translation.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(translation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Translations), new { id = translation.CategoryId });
        }
        return View(translation);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteTranslation(int id)
    {
        var translation = await _context.CategoryTranslations.FindAsync(id);
        if (translation != null)
        {
            var categoryId = translation.CategoryId;
            _context.CategoryTranslations.Remove(translation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Translations), new { id = categoryId });
        }
        return RedirectToAction(nameof(Index));
    }

    private bool CategoryExists(int id)
    {
        return _context.Categories.Any(e => e.Id == id);
    }
}