using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("listening_sessions")]
public class ListeningSession
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

    [Column("audio_id")]
    public Guid? AudioId { get; set; }

    [Column("language_id")]
    public Guid? LanguageId { get; set; }

    [Column("source")]
    [StringLength(50)]
    public string? Source { get; set; }

    [Required]
    [Column("start_time")]
    public DateTime StartTime { get; set; }

    [Column("end_time")]
    public DateTime? EndTime { get; set; }

    [Column("duration_seconds")]
    public int? DurationSeconds { get; set; }

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }

    [ForeignKey("PoiId")]
    public Poi? Poi { get; set; }

    [ForeignKey("AudioId")]
    public Audio? Audio { get; set; }

    [ForeignKey("LanguageId")]
    public Language? Language { get; set; }
}
