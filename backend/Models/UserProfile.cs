using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("user_profiles")]
public class UserProfile
{
    [Key]
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("avatar_url")]
    [Url]
    public string? AvatarUrl { get; set; }

    [Column("preferred_language_id")]
    public Guid? PreferredLanguageId { get; set; }

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("PreferredLanguageId")]
    public Language? PreferredLanguage { get; set; }

    public User? User { get; set; }
}
