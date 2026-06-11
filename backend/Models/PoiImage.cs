using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("poi_images")]
public class PoiImage
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("poi_id")]
    public Guid PoiId { get; set; }

    [Required]
    [Column("image_url")]
    public string ImageUrl { get; set; } = null!;

    [Required]
    [Column("sort_order")]
    public int SortOrder { get; set; }

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("PoiId")]
    public Poi? Poi { get; set; }
}
