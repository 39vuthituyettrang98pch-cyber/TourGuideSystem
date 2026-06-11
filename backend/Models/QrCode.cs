using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("qr_codes")]
public class QrCode
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("poi_id")]
    public Guid PoiId { get; set; }

    [Required]
    [Column("qr_value")]
    public string QrValue { get; set; } = null!;

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("PoiId")]
    public Poi? Poi { get; set; }
}
