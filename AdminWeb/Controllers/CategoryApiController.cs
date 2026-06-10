using AdminWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminWeb.Controllers.Api;

[Route("api/[controller]")]
[ApiController]
public class CategoryApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoryApiController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/category
    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        try
        {
            var lang = Request.Query["lang"].ToString();
            if (string.IsNullOrEmpty(lang)) lang = "vi";

            var categories = await _context.Categories
                .Include(c => c.Translations)
                .Where(c => c.Status == "active")
                .ToListAsync();

            var result = categories.Select(c => {
                var t = c.Translations.FirstOrDefault(tr => tr.LanguageCode == lang)
                        ?? c.Translations.FirstOrDefault();
                return new
                {
                    id = c.Id,
                    name = t?.Name ?? "",
                    icon = c.IconUrl
                };
            }).ToList();

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    // GET: api/category/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(int id)
    {
        try
        {
            var category = await _context.Categories
                .Include(c => c.Translations)
                .Include(c => c.PoiCategories)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound(new { success = false, message = "Category not found" });
            }

            return Ok(new
            {
                success = true,
                data = new
                {
                    category.Id,
                    category.Status,
                    category.IconUrl,
                    Translations = category.Translations.Select(t => new
                    {
                        t.LanguageCode,
                        t.Name
                    }),
                    PoiCount = category.PoiCategories.Count
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
}
