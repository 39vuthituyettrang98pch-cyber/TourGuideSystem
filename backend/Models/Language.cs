using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("languages")]
public class Language
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("code")]
    [StringLength(10)]
    public string Code { get; set; } = null!;

    [Required]
    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    public ICollection<PoiTranslation> PoiTranslations { get; set; } = new List<PoiTranslation>();
    public ICollection<Audio> Audios { get; set; } = new List<Audio>();
    public ICollection<UserProfile> UserProfiles { get; set; } = new List<UserProfile>();
    public ICollection<ListeningSession> ListeningSessions { get; set; } = new List<ListeningSession>();
}
