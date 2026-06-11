using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("users")]
public class User
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("role_id")]
    public Guid RoleId { get; set; }

    [ForeignKey("RoleId")]
    public Role? Role { get; set; }

    [Required]
    [EmailAddress]
    [Column("email")]
    [StringLength(255)]
    public string Email { get; set; } = null!;

    [Required]
    [Column("password_hash")]
    public string PasswordHash { get; set; } = null!;

    [Column("full_name")]
    [StringLength(255)]
    public string? FullName { get; set; }

    [Required]
    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    public UserProfile? Profile { get; set; }

    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<UserFavorite> Favorites { get; set; } = new List<UserFavorite>();
    public ICollection<ListeningSession> ListeningSessions { get; set; } = new List<ListeningSession>();
    public ICollection<UserLocationLog> LocationLogs { get; set; } = new List<UserLocationLog>();
    public ICollection<GeofenceEvent> GeofenceEvents { get; set; } = new List<GeofenceEvent>();
    public ICollection<UserNotification> UserNotifications { get; set; } = new List<UserNotification>();
}
