using AdminWeb.Data;
using AdminWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminWeb.Controllers;

[Authorize]
public class AudioController : Controller
{
    private readonly AppDbContext _context;

    public AudioController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var pois = await _context.Pois
            .Include(p => p.Translations)
            .Include(p => p.MediaAssets)
            .ToListAsync();
            
        return View(new AudioManagementViewModel { Pois = pois });
    }
}