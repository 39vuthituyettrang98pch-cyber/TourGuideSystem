using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("poi_translations")]
public class PoiTranslation
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

    [Required]
    [Column("name")]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    [Column("short_description")]
    public string? ShortDescription { get; set; }

    [Column("full_description")]
    public string? FullDescription { get; set; }

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("PoiId")]
    public Poi? Poi { get; set; }

    [ForeignKey("LanguageId")]
    public Language? Language { get; set; }
}
