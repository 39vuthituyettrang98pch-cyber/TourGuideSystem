using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("audios")]
public class Audio
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("poi_id")]
    public Guid PoiId { get; set; }

    [Required]
    [Column("language_id")]
    public Guid LanguageId { get; set; }

    [Column("title")]
    public string? Title { get; set; }

    [Required]
    [Column("audio_url")]
    public string AudioUrl { get; set; } = null!;

    [Column("duration_seconds")]
    public int? DurationSeconds { get; set; }

    [Required]
    [Column("is_generated_by_tts")]
    public bool IsGeneratedByTts { get; set; }

    [Column("tts_voice")]
    public string? TtsVoice { get; set; }

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("PoiId")]
    public Poi? Poi { get; set; }

    [ForeignKey("LanguageId")]
    public Language? Language { get; set; }

    public ICollection<ListeningSession> ListeningSessions { get; set; } = new List<ListeningSession>();
}
