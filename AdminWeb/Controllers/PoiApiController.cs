using AdminWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminWeb.Controllers.Api
{
    [Route("api/poi")]
    [ApiController]
    public class PoiApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PoiApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/poi
        [HttpGet]
        public async Task<IActionResult> GetAllPois()
        {
            try
            {
                // Lấy languageCode từ query string, mặc định "vi"
                var lang = Request.Query["lang"].ToString();
                if (string.IsNullOrEmpty(lang)) lang = "vi";

                var pois = await _context.Pois
                    .Include(p => p.Translations)
                    .ToListAsync();

                var result = pois.Select(p => {
                    var t = p.Translations.FirstOrDefault(tr => tr.LanguageCode == lang)
                            ?? p.Translations.FirstOrDefault();
                    return new
                    {
                        id = p.Id,
                        name = t?.Name ?? "",
                        shortDescription = t?.ShortDescription ?? "",
                        fullDescription = t?.FullDescription ?? "",
                        audioUrl = t?.AudioUrl,
                        coverImageUrl = p.CoverImageUrl,
                        rating = "4.5",
                        duration = "30 phút",
                        tags = new List<string> { "Khám phá" },
                        latitude = p.Latitude,
                        longitude = p.Longitude,
                        radius = 50.0
                    };
                }).ToList();

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}
