using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("tour_pois")]
public class TourPoi
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("tour_id")]
    public Guid TourId { get; set; }

    [Required]
    [Column("poi_id")]
    public Guid PoiId { get; set; }

    [Required]
    [Column("sort_order")]
    public int SortOrder { get; set; }

    [ForeignKey("TourId")]
    public Tour? Tour { get; set; }

    [ForeignKey("PoiId")]
    public Poi? Poi { get; set; }
}
