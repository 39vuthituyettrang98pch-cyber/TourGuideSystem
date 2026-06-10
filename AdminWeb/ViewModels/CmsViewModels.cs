using AdminWeb.Models;

namespace AdminWeb.ViewModels;

public sealed class DashboardViewModel
{
    public int TotalListens { get; set; }
    public int AvgListenSeconds { get; set; }
    public int ActiveTourists { get; set; }
    public string TopPoi { get; set; } = "";
    public List<PlaybackPoint> PlaybackSeries { get; set; } = [];
    public List<HeatmapPoint> HeatmapPoints { get; set; } = [];
    public List<VisitorPlaybackLog> RecentLogs { get; set; } = [];
}

public sealed class PoiManagementViewModel
{
    public List<Poi> Pois { get; set; } = [];
    public PoiForm Form { get; set; } = PoiForm.CreateDefault();
}

public sealed class PoiForm
{
    public int? Id { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public int Radius { get; set; } = 50;
    public string QrCodeToken { get; set; } = "";
    public string Status { get; set; } = "active";
    public List<PoiTranslationForm> Translations { get; set; } = [];

    public static PoiForm CreateDefault() => new()
    {
        Latitude = 10.762622m,
        Longitude = 106.660172m,
        Translations =
        [
            new() { LanguageCode = "vi" },
            new() { LanguageCode = "en" }
        ]
    };
}

public sealed class PoiTranslationForm
{
    public string LanguageCode { get; set; } = "";
    public string Name { get; set; } = "";
    public string? ShortDescription { get; set; }
    public string? FullDescription { get; set; }
    public string? TtsScript { get; set; }
    public string? AudioUrl { get; set; }
    public int AudioDuration { get; set; }
}

public sealed class TourManagementViewModel
{
    public List<Tour> Tours { get; set; } = [];
    public List<Poi> Pois { get; set; } = [];
    public TourForm Form { get; set; } = TourForm.CreateDefault();
}

public sealed class TourForm
{
    public int? Id { get; set; }
    public int EstimatedTime { get; set; } = 60;
    public string Status { get; set; } = "active";
    public List<TourTranslationForm> Translations { get; set; } = [];
    public List<int> PoiIds { get; set; } = [];

    public static TourForm CreateDefault() => new()
    {
        Translations =
        [
            new() { LanguageCode = "vi" },
            new() { LanguageCode = "en" }
        ]
    };
}

public sealed class TourTranslationForm
{
    public string LanguageCode { get; set; } = "";
    public string Title { get; set; } = "";
    public string? Description { get; set; }
}

public sealed class CategoryManagementViewModel
{
    public List<Category> Categories { get; set; } = [];
}

public sealed class AudioManagementViewModel
{
    public List<Poi> Pois { get; set; } = [];
}

public sealed class UsageHistoryViewModel
{
    public List<VisitorPlaybackLog> Logs { get; set; } = [];
}

public sealed class SettingsViewModel
{
    public List<SystemSetting> Settings { get; set; } = [];
    public List<DataSyncVersion> SyncVersions { get; set; } = [];
}
