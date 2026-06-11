using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("pois")]
public class Poi
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("code")]
    [StringLength(100)]
    public string? Code { get; set; }

    [Required]
    [Column("latitude")]
    public decimal Latitude { get; set; }

    [Required]
    [Column("longitude")]
    public decimal Longitude { get; set; }

    [Column("trigger_radius")]
    public decimal? TriggerRadius { get; set; }

    [Required]
    [Column("priority")]
    public int Priority { get; set; }

    [Column("thumbnail_url")]
    public string? ThumbnailUrl { get; set; }

    [Column("map_url")]
    public string? MapUrl { get; set; }

    [Required]
    [Column("is_active")]
    public bool IsActive { get; set; }

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Required]
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    public ICollection<PoiTranslation> Translations { get; set; } = new List<PoiTranslation>();
    public ICollection<PoiImage> Images { get; set; } = new List<PoiImage>();
    public ICollection<Audio> Audios { get; set; } = new List<Audio>();
    public ICollection<QrCode> QrCodes { get; set; } = new List<QrCode>();
    public ICollection<TourPoi> TourPois { get; set; } = new List<TourPoi>();
    public ICollection<UserFavorite> Favorites { get; set; } = new List<UserFavorite>();
    public ICollection<ListeningSession> ListeningSessions { get; set; } = new List<ListeningSession>();
    public ICollection<GeofenceEvent> GeofenceEvents { get; set; } = new List<GeofenceEvent>();
}
