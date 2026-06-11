using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("tours")]
public class Tour
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("name")]
    public string Name { get; set; } = null!;

    [Column("thumbnail_url")]
    public string? ThumbnailUrl { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("estimated_duration")]
    public int? EstimatedDuration { get; set; }

    [Required]
    [Column("is_active")]
    public bool IsActive { get; set; }

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    public ICollection<TourPoi> TourPois { get; set; } = new List<TourPoi>();
}
