using AdminWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminWeb.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Core
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Tourist> Tourists { get; set; }

    // Data
    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
    public DbSet<Poi> Pois { get; set; }
    public DbSet<PoiTranslation> PoiTranslations { get; set; }
    public DbSet<PoiCategory> PoiCategories { get; set; }
    public DbSet<MediaAsset> MediaAssets { get; set; }
    public DbSet<Beacon> Beacons { get; set; }

    // Tours
    public DbSet<Tour> Tours { get; set; }
    public DbSet<TourTranslation> TourTranslations { get; set; }
    public DbSet<TourPoi> TourPois { get; set; }

    // Interactions & Logs
    public DbSet<Review> Reviews { get; set; }
    public DbSet<TouristFavorite> TouristFavorites { get; set; }
    public DbSet<VisitorPlaybackLog> VisitorPlaybackLogs { get; set; }

    // System
    public DbSet<DataSyncVersion> DataSyncVersions { get; set; }
    public DbSet<AdminActivityLog> AdminActivityLogs { get; set; }
    public DbSet<SystemSetting> SystemSettings { get; set; }
    public DbSet<SyncVersion> SyncVersions { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Setup Table Names
        modelBuilder.Entity<Role>().ToTable("roles");
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Tourist>().ToTable("tourists");
        modelBuilder.Entity<Category>().ToTable("categories");
        modelBuilder.Entity<CategoryTranslation>().ToTable("category_translations");
        modelBuilder.Entity<Poi>().ToTable("pois");
        modelBuilder.Entity<PoiTranslation>().ToTable("poi_translations");
        modelBuilder.Entity<PoiCategory>().ToTable("poi_categories");
        modelBuilder.Entity<MediaAsset>().ToTable("media_assets");
        modelBuilder.Entity<Beacon>().ToTable("beacons");
        modelBuilder.Entity<Tour>().ToTable("tours");
        modelBuilder.Entity<TourTranslation>().ToTable("tour_translations");
        modelBuilder.Entity<TourPoi>().ToTable("tour_pois");
        modelBuilder.Entity<Review>().ToTable("reviews");
        modelBuilder.Entity<TouristFavorite>().ToTable("tourist_favorites");
        modelBuilder.Entity<VisitorPlaybackLog>().ToTable("visitor_playback_logs");
        modelBuilder.Entity<DataSyncVersion>().ToTable("data_sync_versions");
        modelBuilder.Entity<AdminActivityLog>().ToTable("admin_activity_logs");
        modelBuilder.Entity<SystemSetting>().ToTable("system_settings");
        modelBuilder.Entity<SyncVersion>().ToTable("sync_versions");

        // Composite Keys
        modelBuilder.Entity<PoiCategory>()
            .HasKey(pc => new { pc.PoiId, pc.CategoryId });

        modelBuilder.Entity<TourPoi>()
            .HasKey(tp => new { tp.TourId, tp.PoiId });

        modelBuilder.Entity<TouristFavorite>()
            .HasKey(tf => new { tf.TouristId, tf.TargetType, tf.TargetId });

        // Setup Decimal precision for coordinates
        modelBuilder.Entity<Poi>()
            .Property(p => p.Latitude).HasColumnType("decimal(10,8)");
        modelBuilder.Entity<Poi>()
            .Property(p => p.Longitude).HasColumnType("decimal(11,8)");

        modelBuilder.Entity<VisitorPlaybackLog>()
            .Property(v => v.VisitorLatitude).HasColumnType("decimal(10,8)");
        modelBuilder.Entity<VisitorPlaybackLog>()
            .Property(v => v.VisitorLongitude).HasColumnType("decimal(11,8)");
    }
}
