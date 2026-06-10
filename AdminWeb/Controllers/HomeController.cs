using AdminWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminWeb.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Thống kê tổng quan
        ViewBag.TotalPois = await _context.Pois.CountAsync();
        ViewBag.TotalTours = await _context.Tours.CountAsync();
        ViewBag.TotalListens = await _context.VisitorPlaybackLogs.CountAsync();
        ViewBag.TotalAudioFiles = await _context.PoiTranslations.CountAsync(pt => !string.IsNullOrEmpty(pt.AudioUrl));

        // Dữ liệu cho biểu đồ Lượt nghe 7 ngày qua
        var sevenDaysAgo = DateTime.Now.Date.AddDays(-6);
        var dailyLogs = await _context.VisitorPlaybackLogs
            .Where(l => l.CreatedAt >= sevenDaysAgo)
            .GroupBy(l => l.CreatedAt.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToListAsync();

        var chartLabels = new List<string>();
        var chartData = new List<int>();
        for (int i = 0; i < 7; i++)
        {
            var date = sevenDaysAgo.AddDays(i);
            chartLabels.Add(date.ToString("dd/MM"));
            var match = dailyLogs.FirstOrDefault(d => d.Date == date);
            chartData.Add(match?.Count ?? 0);
        }
        ViewBag.ChartLabels = chartLabels;
        ViewBag.ChartData = chartData;

        // Dữ liệu cho biểu đồ tròn (Tỷ lệ ngôn ngữ)
        var languageStats = await _context.VisitorPlaybackLogs
            .GroupBy(l => l.LanguageCode)
            .Select(g => new { Language = g.Key, Count = g.Count() })
            .ToListAsync();
        ViewBag.PieLabels = languageStats.Select(s => (s.Language ?? "unknown").ToUpper()).ToList();
        ViewBag.PieData = languageStats.Select(s => s.Count).ToList();

        // Top 5 POI
        var topPoisData = await _context.VisitorPlaybackLogs
            .GroupBy(l => l.PoiId)
            .Select(g => new { PoiId = g.Key, ListenCount = g.Count() })
            .OrderByDescending(x => x.ListenCount)
            .Take(5)
            .ToListAsync();
        var topPoisDict = new Dictionary<string, int>();
        foreach (var item in topPoisData)
        {
            var poi = await _context.Pois.Include(p => p.Translations).FirstOrDefaultAsync(p => p.Id == item.PoiId);
            var poiName = poi?.Translations?.FirstOrDefault(t => t.LanguageCode == "vi")?.Name ?? $"POI #{item.PoiId}";
            topPoisDict[poiName] = item.ListenCount;
        }
        ViewBag.TopPois = topPoisDict;

        // Hoạt động gần đây
        var recentActivities = await _context.VisitorPlaybackLogs.Include(l => l.Poi).ThenInclude(p => p!.Translations).OrderByDescending(l => l.CreatedAt).Take(5).ToListAsync();
        foreach (var log in recentActivities) log.PoiName = log.Poi?.Translations?.FirstOrDefault(t => t.LanguageCode == "vi")?.Name ?? $"POI #{log.PoiId}";
        ViewBag.RecentActivities = recentActivities;

        // Heatmap
        ViewBag.HeatmapData = await _context.VisitorPlaybackLogs.Where(l => l.VisitorLatitude != null && l.VisitorLongitude != null).Select(l => new { lat = l.VisitorLatitude, lng = l.VisitorLongitude, intensity = 1 }).ToListAsync();

        return View();
    }
}