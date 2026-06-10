using AdminWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AdminWeb.Controllers.Api;

[Route("api/[controller]")]
[ApiController]
public class TourApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public TourApiController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/tour
    [HttpGet]
    public async Task<IActionResult> GetTours([FromQuery] string? keyword, [FromQuery] int? categoryId)
    {
        var lang = Request.Query["lang"].ToString();
        if (string.IsNullOrEmpty(lang)) lang = "vi";

        var query = _context.Tours
            .Where(t => t.Status == "active")
            .AsQueryable();

        if (categoryId.HasValue)
        {
            query = query.Where(t => t.TourPois.Any(tp => tp.Poi!.PoiCategories!.Any(pc => pc.CategoryId == categoryId.Value)));
        }

        if (!string.IsNullOrEmpty(keyword))
        {
            var lowerKeyword = keyword.ToLower();
            query = query.Where(t => t.Translations.Any(tr => tr.Title.ToLower().Contains(lowerKeyword)));
        }

        var tours = await query.ToListAsync();

        var result = tours.Select(t => {
            var tr = t.Translations.FirstOrDefault(x => x.LanguageCode == lang) ?? t.Translations.FirstOrDefault();
            var pois = t.TourPois.OrderBy(tp => tp.SequenceOrder).Select(tp => {
                var ptr = tp.Poi!.Translations.FirstOrDefault(p => p.LanguageCode == lang) ?? tp.Poi.Translations.FirstOrDefault();
                return new {
                    id = tp.Poi.Id,
                    name = ptr?.Name ?? "",
                    shortDescription = ptr?.ShortDescription ?? "",
                    fullDescription = ptr?.FullDescription ?? "",
                    audioUrl = ptr?.AudioUrl,
                    coverImageUrl = tp.Poi.CoverImageUrl,
                    rating = "4.5",
                    duration = "30 phút",
                    tags = new List<string> { "Khám phá" },
                    latitude = tp.Poi.Latitude,
                    longitude = tp.Poi.Longitude,
                    radius = tp.Poi.Radius
                };
            }).ToList();

            return new {
                id = t.Id,
                title = tr?.Title ?? "",
                description = tr?.Description ?? "",
                duration = t.EstimatedTime,
                status = t.Status,
                pois = pois
            };
        }).ToList();

        return Ok(result);
    }

    // GET: api/tour/{tourId}/pois
    [HttpGet("{tourId}/pois")]
    public async Task<IActionResult> GetTourPois(int tourId)
    {
        var lang = Request.Query["lang"].ToString();
        if (string.IsNullOrEmpty(lang)) lang = "vi";

        var tour = await _context.Tours
            .Include(t => t.TourPois)
            .ThenInclude(tp => tp.Poi)
            .ThenInclude(p => p!.Translations)
            .FirstOrDefaultAsync(t => t.Id == tourId);

        if (tour == null)
        {
            return Ok(new { success = false, message = "Tour không tồn tại" });
        }

        var pois = tour.TourPois
            .OrderBy(tp => tp.SequenceOrder)
            .Select(tp => {
                var ptr = tp.Poi!.Translations.FirstOrDefault(p => p.LanguageCode == lang)
                        ?? tp.Poi.Translations.FirstOrDefault();
                return new {
                    id = tp.Poi.Id,
                    name = ptr?.Name ?? "",
                    shortDescription = ptr?.ShortDescription ?? "",
                    fullDescription = ptr?.FullDescription ?? "",
                    audioUrl = ptr?.AudioUrl,
                    coverImageUrl = tp.Poi.CoverImageUrl,
                    rating = "4.5",
                    duration = "30 phút",
                    tags = new List<string> { "Khám phá" },
                    latitude = tp.Poi.Latitude,
                    longitude = tp.Poi.Longitude,
                    radius = tp.Poi.Radius
                };
            })
            .ToList();

        return Ok(pois);
    }
}