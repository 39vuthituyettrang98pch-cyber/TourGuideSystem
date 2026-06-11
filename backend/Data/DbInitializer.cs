using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Apply migrations and ensure the SQLite database is created with the current model
        await db.Database.MigrateAsync();

        // Seed language data
        if (!await db.Languages.AnyAsync())
        {
            var languages = new[]
            {
                new Language { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Code = "en", Name = "English" },
                new Language { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Code = "vi", Name = "Tiếng Việt" },
                new Language { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Code = "fr", Name = "Français" },
                new Language { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Code = "es", Name = "Español" },
                new Language { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Code = "de", Name = "Deutsch" },
                new Language { Id = Guid.Parse("66666666-6666-6666-6666-666666666666"), Code = "it", Name = "Italiano" },
                new Language { Id = Guid.Parse("77777777-7777-7777-7777-777777777777"), Code = "ja", Name = "日本語" },
                new Language { Id = Guid.Parse("88888888-8888-8888-8888-888888888888"), Code = "ko", Name = "한국어" },
                new Language { Id = Guid.Parse("99999999-9999-9999-9999-999999999999"), Code = "zh", Name = "中文" },
                new Language { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Code = "ru", Name = "Русский" }
            };

            db.Languages.AddRange(languages);
            await db.SaveChangesAsync();
        }

        // Seed roles
        if (!await db.Roles.AnyAsync())
        {
            var roles = new[]
            {
                new Role { Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), Code = "admin", Name = "Administrator" },
                new Role { Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), Code = "user", Name = "Regular User" },
                new Role { Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), Code = "guide", Name = "Tour Guide" },
                new Role { Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), Code = "manager", Name = "Manager" },
                new Role { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), Code = "guest", Name = "Guest" },
                new Role { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Code = "editor", Name = "Editor" },
                new Role { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Code = "viewer", Name = "Viewer" },
                new Role { Id = Guid.Parse("00000000-0000-0000-0000-000000000003"), Code = "auditor", Name = "Auditor" },
                new Role { Id = Guid.Parse("00000000-0000-0000-0000-000000000004"), Code = "operator", Name = "Operator" },
                new Role { Id = Guid.Parse("00000000-0000-0000-0000-000000000005"), Code = "owner", Name = "Owner" }
            };

            db.Roles.AddRange(roles);
            await db.SaveChangesAsync();
        }

        if (!await db.Users.AnyAsync())
        {
            var roleMap = await db.Roles.ToDictionaryAsync(r => r.Code, r => r.Id);
            var now = DateTime.UtcNow;
            var users = new[]
            {
                new User { Id = Guid.Parse("11111111-2222-3333-4444-555555555555"), RoleId = roleMap["admin"], Email = "admin@example.com", PasswordHash = HashPassword("pass123"), FullName = "Admin User", IsActive = true, CreatedAt = now },
                new User { Id = Guid.Parse("22222222-3333-4444-5555-666666666666"), RoleId = roleMap["user"], Email = "user1@example.com", PasswordHash = HashPassword("pass123"), FullName = "User One", IsActive = true, CreatedAt = now },
                new User { Id = Guid.Parse("33333333-4444-5555-6666-777777777777"), RoleId = roleMap["user"], Email = "user2@example.com", PasswordHash = HashPassword("pass123"), FullName = "User Two", IsActive = true, CreatedAt = now },
                new User { Id = Guid.Parse("44444444-5555-6666-7777-888888888888"), RoleId = roleMap["guide"], Email = "guide1@example.com", PasswordHash = HashPassword("pass123"), FullName = "Guide One", IsActive = true, CreatedAt = now },
                new User { Id = Guid.Parse("55555555-6666-7777-8888-999999999999"), RoleId = roleMap["manager"], Email = "manager1@example.com", PasswordHash = HashPassword("pass123"), FullName = "Manager One", IsActive = true, CreatedAt = now },
                new User { Id = Guid.Parse("66666666-7777-8888-9999-000000000000"), RoleId = roleMap["guest"], Email = "guest1@example.com", PasswordHash = HashPassword("pass123"), FullName = "Guest One", IsActive = true, CreatedAt = now },
                new User { Id = Guid.Parse("77777777-8888-9999-0000-111111111111"), RoleId = roleMap["editor"], Email = "editor1@example.com", PasswordHash = HashPassword("pass123"), FullName = "Editor One", IsActive = true, CreatedAt = now },
                new User { Id = Guid.Parse("88888888-9999-0000-1111-222222222222"), RoleId = roleMap["viewer"], Email = "viewer1@example.com", PasswordHash = HashPassword("pass123"), FullName = "Viewer One", IsActive = true, CreatedAt = now },
                new User { Id = Guid.Parse("99999999-0000-1111-2222-333333333333"), RoleId = roleMap["auditor"], Email = "auditor1@example.com", PasswordHash = HashPassword("pass123"), FullName = "Auditor One", IsActive = true, CreatedAt = now },
                new User { Id = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), RoleId = roleMap["owner"], Email = "owner1@example.com", PasswordHash = HashPassword("pass123"), FullName = "Owner One", IsActive = true, CreatedAt = now }
            };

            db.Users.AddRange(users);
            await db.SaveChangesAsync();
        }

        if (!await db.UserProfiles.AnyAsync())
        {
            var profileLanguage = await db.Languages.FirstAsync();
            var profileUserIds = await db.Users.Select(u => u.Id).ToListAsync();
            var profiles = profileUserIds.Select((userId, index) => new UserProfile
            {
                UserId = userId,
                AvatarUrl = $"https://example.com/avatars/user{index + 1}.png",
                PreferredLanguageId = profileLanguage.Id,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            db.UserProfiles.AddRange(profiles);
            await db.SaveChangesAsync();
        }

        if (!await db.Pois.AnyAsync())
        {
            var pois = Enumerable.Range(1, 10).Select(index => new Poi
            {
                Id = Guid.Parse($"10000000-0000-0000-0000-00000000000{index:x1}"),
                Code = $"POI{index:D2}",
                Latitude = 10.76m + index * 0.001m,
                Longitude = 106.66m + index * 0.001m,
                TriggerRadius = 50m + index,
                Priority = (index % 3) + 1,
                ThumbnailUrl = $"https://example.com/poi{index}.jpg",
                MapUrl = $"https://maps.example.com/poi{index}",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }).ToList();

            db.Pois.AddRange(pois);
            await db.SaveChangesAsync();
        }

        if (!await db.PoiTranslations.AnyAsync())
        {
            var firstLanguage = await db.Languages.FirstAsync();
            var poiIds = await db.Pois.Select(p => p.Id).ToListAsync();
            var translations = poiIds.Select((poiId, index) => new PoiTranslation
            {
                Id = Guid.Parse($"20000000-0000-0000-0000-00000000000{index + 1:x1}"),
                PoiId = poiId,
                LanguageId = firstLanguage.Id,
                Name = $"POI {index + 1} Name",
                ShortDescription = $"Short description for POI {index + 1}",
                FullDescription = $"Full description for POI {index + 1}.",
                CreatedAt = DateTime.UtcNow
            }).ToList();

            db.PoiTranslations.AddRange(translations);
            await db.SaveChangesAsync();
        }

        if (!await db.PoiImages.AnyAsync())
        {
            var poiIds = await db.Pois.Select(p => p.Id).ToListAsync();
            var images = poiIds.Select((poiId, index) => new PoiImage
            {
                Id = Guid.Parse($"30000000-0000-0000-0000-00000000000{index + 1:x1}"),
                PoiId = poiId,
                ImageUrl = $"https://example.com/poi{index + 1}-image.jpg",
                SortOrder = index,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            db.PoiImages.AddRange(images);
            await db.SaveChangesAsync();
        }

        if (!await db.Audios.AnyAsync())
        {
            var poiIds = await db.Pois.Select(p => p.Id).ToListAsync();
            var language = await db.Languages.FirstAsync();
            var audios = poiIds.Select((poiId, index) => new Audio
            {
                Id = Guid.Parse($"40000000-0000-0000-0000-00000000000{index + 1:x1}"),
                PoiId = poiId,
                LanguageId = language.Id,
                Title = $"POI {index + 1} Audio",
                AudioUrl = $"https://example.com/audio{index + 1}.mp3",
                DurationSeconds = 120 + index * 10,
                IsGeneratedByTts = false,
                TtsVoice = null,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            db.Audios.AddRange(audios);
            await db.SaveChangesAsync();
        }

        if (!await db.QrCodes.AnyAsync())
        {
            var poiIds = await db.Pois.Select(p => p.Id).ToListAsync();
            var qrCodes = poiIds.Select((poiId, index) => new QrCode
            {
                Id = Guid.Parse($"50000000-0000-0000-0000-00000000000{index + 1:x1}"),
                PoiId = poiId,
                QrValue = $"QR{index + 1:D4}",
                CreatedAt = DateTime.UtcNow
            }).ToList();

            db.QrCodes.AddRange(qrCodes);
            await db.SaveChangesAsync();
        }

        if (!await db.Tours.AnyAsync())
        {
            var tours = Enumerable.Range(1, 10).Select(index => new Tour
            {
                Id = Guid.Parse($"60000000-0000-0000-0000-00000000000{index + 1:x1}"),
                Name = $"Tour {index}",
                ThumbnailUrl = $"https://example.com/tour{index}.jpg",
                Description = $"Description for Tour {index}",
                EstimatedDuration = 30 + index * 5,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            db.Tours.AddRange(tours);
            await db.SaveChangesAsync();
        }

        if (!await db.TourPois.AnyAsync())
        {
            var tourIds = await db.Tours.Select(t => t.Id).ToListAsync();
            var poiIds = await db.Pois.Select(p => p.Id).ToListAsync();
            var tourPois = Enumerable.Range(0, 10).Select(index => new TourPoi
            {
                Id = Guid.Parse($"70000000-0000-0000-0000-00000000000{index + 1:x1}"),
                TourId = tourIds[index],
                PoiId = poiIds[index],
                SortOrder = index + 1
            }).ToList();

            db.TourPois.AddRange(tourPois);
            await db.SaveChangesAsync();
        }

        if (!await db.UserFavorites.AnyAsync())
        {
            var userIds = await db.Users.Select(u => u.Id).ToListAsync();
            var poiIds = await db.Pois.Select(p => p.Id).ToListAsync();
            var favorites = Enumerable.Range(0, 10).Select(index => new UserFavorite
            {
                UserId = userIds[index],
                PoiId = poiIds[index],
                CreatedAt = DateTime.UtcNow
            }).ToList();

            db.UserFavorites.AddRange(favorites);
            await db.SaveChangesAsync();
        }

        if (!await db.ListeningSessions.AnyAsync())
        {
            var userIds = await db.Users.Select(u => u.Id).ToListAsync();
            var poiIds = await db.Pois.Select(p => p.Id).ToListAsync();
            var audioIds = await db.Audios.Select(a => a.Id).ToListAsync();
            var languageId = await db.Languages.Select(l => l.Id).FirstAsync();
            var sessions = Enumerable.Range(0, 10).Select(index => new ListeningSession
            {
                Id = Guid.Parse($"80000000-0000-0000-0000-00000000000{index + 1:x1}"),
                UserId = userIds[index],
                PoiId = poiIds[index],
                AudioId = audioIds[index],
                LanguageId = languageId,
                Source = "app",
                StartTime = DateTime.UtcNow.AddMinutes(-index * 10),
                EndTime = DateTime.UtcNow.AddMinutes(-index * 10 + 3),
                DurationSeconds = 180,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            db.ListeningSessions.AddRange(sessions);
            await db.SaveChangesAsync();
        }

        if (!await db.UserLocationLogs.AnyAsync())
        {
            var userIds = await db.Users.Select(u => u.Id).ToListAsync();
            var logs = Enumerable.Range(0, 10).Select(index => new UserLocationLog
            {
                Id = Guid.Parse($"90000000-0000-0000-0000-00000000000{index + 1:x1}"),
                UserId = userIds[index],
                Latitude = 10.76m + index * 0.001m,
                Longitude = 106.66m + index * 0.001m,
                RecordedAt = DateTime.UtcNow.AddMinutes(-index * 5)
            }).ToList();

            db.UserLocationLogs.AddRange(logs);
            await db.SaveChangesAsync();
        }

        if (!await db.GeofenceEvents.AnyAsync())
        {
            var userIds = await db.Users.Select(u => u.Id).ToListAsync();
            var poiIds = await db.Pois.Select(p => p.Id).ToListAsync();
            var events = Enumerable.Range(0, 10).Select(index => new GeofenceEvent
            {
                Id = Guid.Parse($"a0000000-0000-0000-0000-00000000000{index + 1:x1}"),
                UserId = userIds[index],
                PoiId = poiIds[index],
                EventType = index % 2 == 0 ? "enter" : "exit",
                Distance = 5m + index,
                EventTime = DateTime.UtcNow.AddMinutes(-index * 2)
            }).ToList();

            db.GeofenceEvents.AddRange(events);
            await db.SaveChangesAsync();
        }

        if (!await db.Notifications.AnyAsync())
        {
            var notifications = Enumerable.Range(1, 10).Select(index => new Notification
            {
                Id = Guid.Parse($"b0000000-0000-0000-0000-00000000000{index:x1}"),
                Title = $"Notification {index}",
                Content = $"This is the content of notification {index}.",
                CreatedAt = DateTime.UtcNow
            }).ToList();

            db.Notifications.AddRange(notifications);
            await db.SaveChangesAsync();
        }

        if (!await db.UserNotifications.AnyAsync())
        {
            var userIds = await db.Users.Select(u => u.Id).ToListAsync();
            var notificationIds = await db.Notifications.Select(n => n.Id).ToListAsync();
            var userNotifications = Enumerable.Range(0, 10).Select(index => new UserNotification
            {
                UserId = userIds[index],
                NotificationId = notificationIds[index],
                IsRead = false,
                ReadAt = null
            }).ToList();

            db.UserNotifications.AddRange(userNotifications);
            await db.SaveChangesAsync();
        }

        static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToHexString(hash);
        }
    }
}
