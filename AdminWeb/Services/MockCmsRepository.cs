using AdminWeb.Models;
using AdminWeb.ViewModels;

namespace AdminWeb.Services;

public sealed class MockCmsRepository : ICmsRepository
{
    private readonly List<Poi> _pois =
    [
        new()
        {
            Id = 1,
            Latitude = 10.762622m,
            Longitude = 106.660172m,
            Radius = 60,
            QrCodeToken = "POI-CH-001",
            Status = "active",
            Translations =
            [
                new() { LanguageCode = "vi", Name = "Dinh Doc Lap", ShortDescription = "Diem tham quan lich su trung tam thanh pho.", FullDescription = "Cong trinh gan voi nhieu dau moc lich su quan trong cua Sai Gon.", AudioUrl = "/audio/dinh-doc-lap-vi.mp3" },
                new() { LanguageCode = "en", Name = "Independence Palace", ShortDescription = "Historic landmark in the city center.", FullDescription = "A signature site connected with key milestones in Saigon history.", AudioUrl = "/audio/independence-palace-en.mp3" }
            ],
            MediaAssets =
            [
                new() { Id = 1, PoiId = 1, MediaType = "image", MediaUrl = "/media/dinh-doc-lap.jpg", SortOrder = 1 },
                new() { Id = 2, PoiId = 1, MediaType = "video", MediaUrl = "/media/dinh-doc-lap.mp4", SortOrder = 2 }
            ],
            Beacons =
            [
                new() { Id = 1, PoiId = 1, Uuid = "fda50693-a4e2-4fb1-afcf-c6eb07647825", Major = 100, Minor = 1, PlacementNote = "Main hall entrance" }
            ]
        },
        new()
        {
            Id = 2,
            Latitude = 10.779784m,
            Longitude = 106.699018m,
            Radius = 45,
            QrCodeToken = "POI-ND-002",
            Status = "active",
            Translations =
            [
                new() { LanguageCode = "vi", Name = "Nha tho Duc Ba", ShortDescription = "Bieu tuong kien truc Phap tai trung tam TP.HCM.", FullDescription = "Khong gian ton giao va kien truc noi bat tren truc duong Cong xa Paris.", AudioUrl = "/audio/nha-tho-duc-ba-vi.mp3" },
                new() { LanguageCode = "en", Name = "Notre-Dame Cathedral Basilica", ShortDescription = "French colonial architectural icon.", FullDescription = "A prominent religious and architectural site on Paris Commune Square.", AudioUrl = "/audio/notre-dame-en.mp3" }
            ],
            MediaAssets =
            [
                new() { Id = 3, PoiId = 2, MediaType = "image", MediaUrl = "/media/notre-dame.jpg", SortOrder = 1 }
            ]
        },
        new()
        {
            Id = 3,
            Latitude = 10.775658m,
            Longitude = 106.700424m,
            Radius = 55,
            QrCodeToken = "POI-BD-003",
            Status = "hidden",
            Translations =
            [
                new() { LanguageCode = "vi", Name = "Buu dien Thanh pho", ShortDescription = "Cong trinh co dien gan Nha tho Duc Ba.", FullDescription = "Diem dung chan quen thuoc voi mat dung vang va khong gian sanh lon.", AudioUrl = "/audio/buu-dien-vi.mp3" },
                new() { LanguageCode = "en", Name = "Central Post Office", ShortDescription = "Classic heritage building near Notre-Dame.", FullDescription = "A familiar stop with a bright facade and grand public hall.", AudioUrl = "/audio/post-office-en.mp3" }
            ]
        }
    ];

    private readonly List<Category> _categories =
    [
        new()
        {
            Id = 1,
            IconUrl = "/icons/history.svg",
            Status = "active",
            Translations =
            [
                new() { LanguageCode = "vi", Name = "Lich su" },
                new() { LanguageCode = "en", Name = "History" }
            ]
        },
        new()
        {
            Id = 2,
            IconUrl = "/icons/architecture.svg",
            Status = "active",
            Translations =
            [
                new() { LanguageCode = "vi", Name = "Kien truc" },
                new() { LanguageCode = "en", Name = "Architecture" }
            ]
        }
    ];

    private readonly List<Tour> _tours =
    [
        new()
        {
            Id = 1,
            EstimatedTime = 90,
            Status = "active",
            PoiIds = [1, 2, 3],
            Translations =
            [
                new() { LanguageCode = "vi", Title = "Dau an Sai Gon xua", Description = "Lo trinh tham quan cac cong trinh bieu tuong cua trung tam thanh pho." },
                new() { LanguageCode = "en", Title = "Old Saigon Highlights", Description = "A route through iconic heritage landmarks in the city center." }
            ]
        },
        new()
        {
            Id = 2,
            EstimatedTime = 45,
            Status = "active",
            PoiIds = [2, 3],
            Translations =
            [
                new() { LanguageCode = "vi", Title = "Kien truc thuoc dia", Description = "Tour ngan tap trung vao cac diem kien truc Phap." },
                new() { LanguageCode = "en", Title = "Colonial Architecture Walk", Description = "A compact tour focused on French colonial architecture." }
            ]
        }
    ];

    private readonly List<VisitorPlaybackLog> _playbackLogs =
    [
        new() { Id = 1, DeviceId = "anon-7F92", PoiId = 1, PoiName = "Dinh Doc Lap", LanguageCode = "vi", TriggerType = "GPS", VisitorLatitude = 10.7628m, VisitorLongitude = 106.6604m, ListenDuration = 210, CreatedAt = DateTime.Now.AddMinutes(-18) },
        new() { Id = 2, DeviceId = "anon-19AC", PoiId = 2, PoiName = "Nha tho Duc Ba", LanguageCode = "en", TriggerType = "QR_CODE", VisitorLatitude = 10.7795m, VisitorLongitude = 106.6992m, ListenDuration = 185, CreatedAt = DateTime.Now.AddHours(-2) },
        new() { Id = 3, DeviceId = "anon-C31B", PoiId = 3, PoiName = "Buu dien Thanh pho", LanguageCode = "vi", TriggerType = "BEACON", VisitorLatitude = 10.7758m, VisitorLongitude = 106.7007m, ListenDuration = 240, CreatedAt = DateTime.Now.AddHours(-4) },
        new() { Id = 4, DeviceId = "anon-AB22", PoiId = 1, PoiName = "Dinh Doc Lap", LanguageCode = "vi", TriggerType = "MANUAL", VisitorLatitude = 10.7761m, VisitorLongitude = 106.6999m, ListenDuration = 166, CreatedAt = DateTime.Now.AddHours(-6) }
    ];

    private readonly List<SystemSetting> _settings =
    [
        new() { Id = 1, SettingKey = "gps_accuracy_meters", SettingValue = "25", Description = "Minimum accepted mobile GPS accuracy" },
        new() { Id = 2, SettingKey = "narration_cooldown_minutes", SettingValue = "10", Description = "Prevent repeated POI playback" },
        new() { Id = 3, SettingKey = "offline_package_enabled", SettingValue = "true", Description = "Allow mobile app to prefetch POI/audio data" }
    ];

    private readonly List<DataSyncVersion> _syncVersions =
    [
        new() { Id = 1, VersionNumber = "2026.06.05.1", ReleaseNotes = "Initial POI, tour and audio catalog.", IsForceUpdate = false, CreatedAt = DateTime.Now.AddDays(-1) },
        new() { Id = 2, VersionNumber = "2026.06.06.1", ReleaseNotes = "Updated Notre-Dame narration and map assets.", IsForceUpdate = true, CreatedAt = DateTime.Now }
    ];

    public IReadOnlyList<Poi> GetPois() => _pois;

    public IReadOnlyList<Tour> GetTours() => _tours;

    public IReadOnlyList<Category> GetCategories() => _categories;

    public IReadOnlyList<VisitorPlaybackLog> GetPlaybackLogs() => _playbackLogs;

    public IReadOnlyList<SystemSetting> GetSettings() => _settings;

    public IReadOnlyList<DataSyncVersion> GetSyncVersions() => _syncVersions;

    public DashboardViewModel GetDashboard() => new()
    {
        TotalListens = 18420,
        AvgListenSeconds = 222,
        ActiveTourists = 1284,
        TopPoi = "Dinh Doc Lap",
        PlaybackSeries =
        [
            new() { Day = "Mon", Listens = 124, AvgListenSeconds = 188 },
            new() { Day = "Tue", Listens = 156, AvgListenSeconds = 201 },
            new() { Day = "Wed", Listens = 142, AvgListenSeconds = 194 },
            new() { Day = "Thu", Listens = 212, AvgListenSeconds = 224 },
            new() { Day = "Fri", Listens = 238, AvgListenSeconds = 231 },
            new() { Day = "Sat", Listens = 316, AvgListenSeconds = 246 },
            new() { Day = "Sun", Listens = 281, AvgListenSeconds = 239 }
        ],
        HeatmapPoints =
        [
            new() { VisitorLatitude = 10.7628m, VisitorLongitude = 106.6604m, ListenDuration = 210 },
            new() { VisitorLatitude = 10.7795m, VisitorLongitude = 106.6992m, ListenDuration = 185 },
            new() { VisitorLatitude = 10.7758m, VisitorLongitude = 106.7007m, ListenDuration = 240 },
            new() { VisitorLatitude = 10.7761m, VisitorLongitude = 106.6999m, ListenDuration = 166 }
        ],
        RecentLogs = _playbackLogs
    };

    public void SavePoi(PoiForm form)
    {
        var poi = form.Id.HasValue ? _pois.FirstOrDefault(item => item.Id == form.Id.Value) : null;
        if (poi is null)
        {
            poi = new Poi { Id = _pois.Count == 0 ? 1 : _pois.Max(item => item.Id) + 1 };
            _pois.Insert(0, poi);
        }

        poi.Latitude = form.Latitude;
        poi.Longitude = form.Longitude;
        poi.Radius = form.Radius;
        poi.QrCodeToken = string.IsNullOrWhiteSpace(form.QrCodeToken) ? $"POI-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}" : form.QrCodeToken;
        poi.Status = form.Status;
        poi.Translations = form.Translations.Select(item => new PoiTranslation
        {
            LanguageCode = item.LanguageCode,
            Name = item.Name,
            ShortDescription = item.ShortDescription,
            FullDescription = item.FullDescription,
            AudioUrl = item.AudioUrl
            ,
            TtsScript = item.TtsScript,
            AudioDuration = item.AudioDuration
        }).ToList();
    }

    public void SaveTour(TourForm form)
    {
        var tour = form.Id.HasValue ? _tours.FirstOrDefault(item => item.Id == form.Id.Value) : null;
        if (tour is null)
        {
            tour = new Tour { Id = _tours.Count == 0 ? 1 : _tours.Max(item => item.Id) + 1 };
            _tours.Insert(0, tour);
        }

        tour.EstimatedTime = form.EstimatedTime;
        tour.Status = form.Status;
        tour.PoiIds = form.PoiIds;
        tour.Translations = form.Translations.Select(item => new TourTranslation
        {
            LanguageCode = item.LanguageCode,
            Title = item.Title,
            Description = item.Description
        }).ToList();
    }
}
