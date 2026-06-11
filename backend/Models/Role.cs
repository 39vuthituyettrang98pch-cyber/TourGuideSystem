using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("roles")]
public class Role
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("code")]
    [StringLength(50)]
    public string Code { get; set; } = null!;

    [Required]
    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;
}
