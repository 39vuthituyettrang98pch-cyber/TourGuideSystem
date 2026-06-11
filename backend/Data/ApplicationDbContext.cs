using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public DbSet<UserProfile> UserProfiles { get; set; } = null!;
    public DbSet<Language> Languages { get; set; } = null!;
    public DbSet<Poi> Pois { get; set; } = null!;
    public DbSet<PoiTranslation> PoiTranslations { get; set; } = null!;
    public DbSet<PoiImage> PoiImages { get; set; } = null!;
    public DbSet<Audio> Audios { get; set; } = null!;
    public DbSet<QrCode> QrCodes { get; set; } = null!;
    public DbSet<Tour> Tours { get; set; } = null!;
    public DbSet<TourPoi> TourPois { get; set; } = null!;
    public DbSet<UserFavorite> UserFavorites { get; set; } = null!;
    public DbSet<ListeningSession> ListeningSessions { get; set; } = null!;
    public DbSet<UserLocationLog> UserLocationLogs { get; set; } = null!;
    public DbSet<GeofenceEvent> GeofenceEvents { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<UserNotification> UserNotifications { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Language>(entity =>
        {
            entity.ToTable("languages");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code").IsRequired().HasMaxLength(10);
            entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Code).IsUnique();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("roles");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code").IsRequired().HasMaxLength(50);
            entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Code).IsUnique();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RoleId).HasColumnName("role_id").IsRequired();
            entity.Property(e => e.Email).HasColumnName("email").IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired();
            entity.Property(e => e.FullName).HasColumnName("full_name").HasMaxLength(255);
            entity.Property(e => e.IsActive).HasColumnName("is_active").IsRequired().HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(e => e.Role).WithMany().HasForeignKey(e => e.RoleId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Profile).WithOne(p => p.User).HasForeignKey<UserProfile>(p => p.UserId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.RefreshTokens).WithOne(r => r.User).HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.Favorites).WithOne(f => f.User).HasForeignKey(f => f.UserId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.ListeningSessions).WithOne(ls => ls.User).HasForeignKey(ls => ls.UserId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.LocationLogs).WithOne(log => log.User).HasForeignKey(log => log.UserId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.GeofenceEvents).WithOne(evt => evt.User).HasForeignKey(evt => evt.UserId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.UserNotifications).WithOne(un => un.User).HasForeignKey(un => un.UserId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("refresh_tokens");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
            entity.Property(e => e.Token).HasColumnName("token").IsRequired();
            entity.HasIndex(e => e.Token).IsUnique();
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at").IsRequired();
            entity.Property(e => e.IsRevoked).HasColumnName("is_revoked").IsRequired().HasDefaultValue(false);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.ToTable("user_profiles");
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.AvatarUrl).HasColumnName("avatar_url");
            entity.Property(e => e.PreferredLanguageId).HasColumnName("preferred_language_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();
            entity.HasOne(e => e.PreferredLanguage).WithMany(l => l.UserProfiles).HasForeignKey(e => e.PreferredLanguageId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Poi>(entity =>
        {
            entity.ToTable("pois");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code").IsRequired().HasMaxLength(100);
            entity.Property(e => e.Latitude).HasColumnName("latitude").IsRequired();
            entity.Property(e => e.Longitude).HasColumnName("longitude").IsRequired();
            entity.Property(e => e.TriggerRadius).HasColumnName("trigger_radius").IsRequired();
            entity.Property(e => e.Priority).HasColumnName("priority").IsRequired();
            entity.Property(e => e.ThumbnailUrl).HasColumnName("thumbnail_url").IsRequired();
            entity.Property(e => e.MapUrl).HasColumnName("map_url").IsRequired();
            entity.Property(e => e.IsActive).HasColumnName("is_active").IsRequired().HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").IsRequired();
        });

        modelBuilder.Entity<PoiTranslation>(entity =>
        {
            entity.ToTable("poi_translations");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PoiId).HasColumnName("poi_id").IsRequired();
            entity.Property(e => e.LanguageId).HasColumnName("language_id").IsRequired();
            entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(200);
            entity.Property(e => e.ShortDescription).HasColumnName("short_description").IsRequired();
            entity.Property(e => e.FullDescription).HasColumnName("full_description").IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();
            entity.HasIndex(e => new { e.PoiId, e.LanguageId }).IsUnique();
            entity.HasOne(e => e.Poi).WithMany(p => p.Translations).HasForeignKey(e => e.PoiId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Language).WithMany(l => l.PoiTranslations).HasForeignKey(e => e.LanguageId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PoiImage>(entity =>
        {
            entity.ToTable("poi_images");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PoiId).HasColumnName("poi_id").IsRequired();
            entity.Property(e => e.ImageUrl).HasColumnName("image_url").IsRequired();
            entity.Property(e => e.SortOrder).HasColumnName("sort_order").IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();
            entity.HasOne(e => e.Poi).WithMany(p => p.Images).HasForeignKey(e => e.PoiId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Audio>(entity =>
        {
            entity.ToTable("audios");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PoiId).HasColumnName("poi_id").IsRequired();
            entity.Property(e => e.LanguageId).HasColumnName("language_id").IsRequired();
            entity.Property(e => e.Title).HasColumnName("title").IsRequired();
            entity.Property(e => e.AudioUrl).HasColumnName("audio_url").IsRequired();
            entity.Property(e => e.DurationSeconds).HasColumnName("duration_seconds").IsRequired();
            entity.Property(e => e.IsGeneratedByTts).HasColumnName("is_generated_by_tts").IsRequired();
            entity.Property(e => e.TtsVoice).HasColumnName("tts_voice");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();
            entity.HasIndex(e => new { e.PoiId, e.LanguageId }).IsUnique();
            entity.HasOne(e => e.Poi).WithMany(p => p.Audios).HasForeignKey(e => e.PoiId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Language).WithMany(l => l.Audios).HasForeignKey(e => e.LanguageId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<QrCode>(entity =>
        {
            entity.ToTable("qr_codes");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PoiId).HasColumnName("poi_id").IsRequired();
            entity.Property(e => e.QrValue).HasColumnName("qr_value").IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();
            entity.HasOne(e => e.Poi).WithMany(p => p.QrCodes).HasForeignKey(e => e.PoiId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Tour>(entity =>
        {
            entity.ToTable("tours");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired();
            entity.Property(e => e.ThumbnailUrl).HasColumnName("thumbnail_url").IsRequired();
            entity.Property(e => e.Description).HasColumnName("description").IsRequired();
            entity.Property(e => e.EstimatedDuration).HasColumnName("estimated_duration").IsRequired();
            entity.Property(e => e.IsActive).HasColumnName("is_active").IsRequired().HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();
        });

        modelBuilder.Entity<TourPoi>(entity =>
        {
            entity.ToTable("tour_pois");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TourId).HasColumnName("tour_id").IsRequired();
            entity.Property(e => e.PoiId).HasColumnName("poi_id").IsRequired();
            entity.Property(e => e.SortOrder).HasColumnName("sort_order").IsRequired();
            entity.HasOne(e => e.Tour).WithMany(t => t.TourPois).HasForeignKey(e => e.TourId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Poi).WithMany(p => p.TourPois).HasForeignKey(e => e.PoiId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserFavorite>(entity =>
        {
            entity.ToTable("user_favorites");
            entity.HasKey(e => new { e.UserId, e.PoiId });
            entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
            entity.Property(e => e.PoiId).HasColumnName("poi_id").IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();
            entity.HasOne(e => e.User).WithMany(u => u.Favorites).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Poi).WithMany(p => p.Favorites).HasForeignKey(e => e.PoiId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ListeningSession>(entity =>
        {
            entity.ToTable("listening_sessions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
            entity.Property(e => e.PoiId).HasColumnName("poi_id").IsRequired();
            entity.Property(e => e.AudioId).HasColumnName("audio_id").IsRequired();
            entity.Property(e => e.LanguageId).HasColumnName("language_id").IsRequired();
            entity.Property(e => e.Source).HasColumnName("source").IsRequired().HasMaxLength(50);
            entity.Property(e => e.StartTime).HasColumnName("start_time").IsRequired();
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.DurationSeconds).HasColumnName("duration_seconds");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();
            entity.HasOne(e => e.User).WithMany(u => u.ListeningSessions).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Poi).WithMany(p => p.ListeningSessions).HasForeignKey(e => e.PoiId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Audio).WithMany(a => a.ListeningSessions).HasForeignKey(e => e.AudioId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Language).WithMany(l => l.ListeningSessions).HasForeignKey(e => e.LanguageId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<UserLocationLog>(entity =>
        {
            entity.ToTable("user_location_logs");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
            entity.Property(e => e.Latitude).HasColumnName("latitude").IsRequired();
            entity.Property(e => e.Longitude).HasColumnName("longitude").IsRequired();
            entity.Property(e => e.RecordedAt).HasColumnName("recorded_at").IsRequired();
            entity.HasOne(e => e.User).WithMany(u => u.LocationLogs).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<GeofenceEvent>(entity =>
        {
            entity.ToTable("geofence_events");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
            entity.Property(e => e.PoiId).HasColumnName("poi_id").IsRequired();
            entity.Property(e => e.EventType).HasColumnName("event_type").IsRequired();
            entity.Property(e => e.Distance).HasColumnName("distance").IsRequired();
            entity.Property(e => e.EventTime).HasColumnName("event_time").IsRequired();
            entity.HasOne(e => e.User).WithMany(u => u.GeofenceEvents).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Poi).WithMany(p => p.GeofenceEvents).HasForeignKey(e => e.PoiId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("notifications");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title).HasColumnName("title").IsRequired();
            entity.Property(e => e.Content).HasColumnName("content").IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();
        });

        modelBuilder.Entity<UserNotification>(entity =>
        {
            entity.ToTable("user_notifications");
            entity.HasKey(e => new { e.UserId, e.NotificationId });
            entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
            entity.Property(e => e.NotificationId).HasColumnName("notification_id").IsRequired();
            entity.Property(e => e.IsRead).HasColumnName("is_read").IsRequired().HasDefaultValue(false);
            entity.Property(e => e.ReadAt).HasColumnName("read_at");
            entity.HasOne(e => e.User).WithMany(u => u.UserNotifications).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Notification).WithMany(n => n.UserNotifications).HasForeignKey(e => e.NotificationId).OnDelete(DeleteBehavior.Cascade);
        });
    }
}
