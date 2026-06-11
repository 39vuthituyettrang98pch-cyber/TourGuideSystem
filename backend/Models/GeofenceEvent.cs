using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("geofence_events")]
public class GeofenceEvent
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Required]
    [Column("poi_id")]
    public Guid PoiId { get; set; }

    [Required]
    [Column("event_type")]
    public string EventType { get; set; } = null!;

    [Column("distance")]
    public decimal? Distance { get; set; }

    [Required]
    [Column("event_time")]
    public DateTime EventTime { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }

    [ForeignKey("PoiId")]
    public Poi? Poi { get; set; }
}
