using System.ComponentModel.DataAnnotations.Schema;

namespace AdminWeb.Models; // DÀNH CHO WEB

public sealed class Role
{
    public int Id { get; set; }
    public string RoleName { get; set; } = "";
    public string? Description { get; set; }
    public List<User> Users { get; set; } = [];
}

public sealed class User
{
    public int Id { get; set; }
    public int? RoleId { get; set; }
    public string Username { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string Email { get; set; } = "";
    public string Status { get; set; } = "active";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public Role? Role { get; set; }
}

public sealed class Tourist
{
    public int Id { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public string? FullName { get; set; }
    public string AuthProvider { get; set; } = "local";
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public List<Review> Reviews { get; set; } = [];
    public List<TouristFavorite> Favorites { get; set; } = [];
    public List<VisitorPlaybackLog> PlaybackLogs { get; set; } = [];
}

public sealed class Poi
{
    public int Id { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public int Radius { get; set; } = 50;
    public string QrCodeToken { get; set; } = "";
    public string Status { get; set; } = "active";
    public int? CreatedBy { get; set; }
    public string? CoverImageUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public User? Creator { get; set; }
    public List<PoiTranslation> Translations { get; set; } = [];
    public List<MediaAsset> MediaAssets { get; set; } = [];
    public List<Beacon> Beacons { get; set; } = [];
    public List<PoiCategory> PoiCategories { get; set; } = [];
    public List<TourPoi> TourPois { get; set; } = [];
    public List<VisitorPlaybackLog> PlaybackLogs { get; set; } = [];
}

public sealed class PoiTranslation
{
    public int Id { get; set; }
    public int PoiId { get; set; }
    public string LanguageCode { get; set; } = "vi";
    public string Name { get; set; } = "";
    public string? ShortDescription { get; set; }
    public string? FullDescription { get; set; }
    public string? TtsScript { get; set; }
    public string? AudioUrl { get; set; }
    public int AudioDuration { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public Poi? Poi { get; set; }
}

public sealed class Category
{
    public int Id { get; set; }
    public string? IconUrl { get; set; }
    public string Status { get; set; } = "active";
    public List<CategoryTranslation> Translations { get; set; } = [];
    public List<PoiCategory> PoiCategories { get; set; } = [];
}

public sealed class CategoryTranslation
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string LanguageCode { get; set; } = "vi";
    public string Name { get; set; } = "";
    public Category? Category { get; set; }
}

public sealed class PoiCategory
{
    public int PoiId { get; set; }
    public Poi? Poi { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}

public sealed class MediaAsset
{
    public int Id { get; set; }
    public int PoiId { get; set; }
    public string MediaType { get; set; } = "image";
    public string MediaUrl { get; set; } = "";
    public int SortOrder { get; set; }
    public Poi? Poi { get; set; }
}

public sealed class Beacon
{
    public int Id { get; set; }
    public int PoiId { get; set; }
    public string? MacAddress { get; set; }
    public string Uuid { get; set; } = "";
    public int Major { get; set; }
    public int Minor { get; set; }
    public string? PlacementNote { get; set; }
    public Poi? Poi { get; set; }
}

public sealed class Tour
{
    public int Id { get; set; }
    public int EstimatedTime { get; set; }
    public string Status { get; set; } = "active";
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public List<TourTranslation> Translations { get; set; } = [];
    public List<TourPoi> TourPois { get; set; } = [];

    [NotMapped]
    public List<int> PoiIds { get; set; } = [];
}

public sealed class TourTranslation
{
    public int Id { get; set; }
    public int TourId { get; set; }
    public string LanguageCode { get; set; } = "vi";
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public Tour? Tour { get; set; }
}

public sealed class TourPoi
{
    public int TourId { get; set; }
    public Tour? Tour { get; set; }
    public int PoiId { get; set; }
    public Poi? Poi { get; set; }
    public int SequenceOrder { get; set; }
}

public sealed class Review
{
    public int Id { get; set; }
    public int? TouristId { get; set; }
    public string TargetType { get; set; } = "POI";
    public int TargetId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public Tourist? Tourist { get; set; }
}

public sealed class TouristFavorite
{
    public int TouristId { get; set; }
    public Tourist? Tourist { get; set; }
    public string TargetType { get; set; } = "POI";
    public int TargetId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public sealed class PlaybackPoint
{
    public string Day { get; set; } = "";
    public int Listens { get; set; }
    public int AvgListenSeconds { get; set; }
}

public sealed class HeatmapPoint
{
    public decimal VisitorLatitude { get; set; }
    public decimal VisitorLongitude { get; set; }
    public int ListenDuration { get; set; }
}

public sealed class VisitorPlaybackLog
{
    public long Id { get; set; }
    public int? TouristId { get; set; }
    public string DeviceId { get; set; } = "";
    public int PoiId { get; set; }
    public string LanguageCode { get; set; } = "vi";
    public string TriggerType { get; set; } = "GPS";
    public decimal? VisitorLatitude { get; set; }
    public decimal? VisitorLongitude { get; set; }
    public int ListenDuration { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Poi? Poi { get; set; }
    public Tourist? Tourist { get; set; }

    [NotMapped]
    public string? PoiName { get; set; }
}

public sealed class SystemSetting
{
    public int Id { get; set; }
    public string SettingKey { get; set; } = "";
    public string? SettingValue { get; set; }
    public string? Description { get; set; }
}

public sealed class DataSyncVersion
{
    public int Id { get; set; }
    public string VersionNumber { get; set; } = "";
    public string? ReleaseNotes { get; set; }
    public bool IsForceUpdate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public sealed class AdminActivityLog
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string Action { get; set; } = "";
    public string? TargetTable { get; set; }
    public int? TargetId { get; set; }
    public string? Description { get; set; }
    public string? IpAddress { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public User? User { get; set; }
}