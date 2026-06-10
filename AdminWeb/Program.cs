using AdminWeb.Data;
using AdminWeb.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization; // Quan trọng để xử lý JSON

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// [FIX LỖI JSON]: Cấu hình IgnoreCycles để ngăn vòng lặp khi lấy dữ liệu từ DB
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// [CẤU HÌNH CORS]: Siết CORS để giảm rủi ro bảo mật.
// Mặc định cho phép origin localhost. Bạn có thể mở rộng thêm trong appsettings nếu cần.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowTrustedOrigins",
        policy =>
        {
            policy.WithOrigins(
                    "http://localhost:5173",
                    "http://localhost:3000",
                    "https://localhost:5173",
                    "https://localhost:3000")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
});

// Database
var defaultConn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (!string.IsNullOrEmpty(defaultConn) && defaultConn.Contains("Data Source="))
    {
        options.UseSqlite(defaultConn);
    }
    else
    {
        options.UseSqlServer(defaultConn);
    }
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// [KÍCH HOẠT CORS]: Phải để giữa UseRouting và UseAuthentication
app.UseCors("AllowTrustedOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Seed admin user and role
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<AppDbContext>();
        db.Database.Migrate();

        var adminRole = db.Roles.FirstOrDefault(r => r.RoleName == "Admin");
        if (adminRole == null)
        {
            adminRole = new Role { RoleName = "Admin", Description = "Administrator" };
            db.Roles.Add(adminRole);
            db.SaveChanges();
        }

        var adminUser = db.Users.FirstOrDefault(u => u.Username == "admin");
        if (adminUser == null)
        {
            adminUser = new User
            {
                Username = "admin",
                PasswordHash = ComputeSha256Hash("admin"),
                RoleId = adminRole.Id,
                Email = "admin@local",
                Status = "active",
                CreatedAt = DateTime.Now
            };
            db.Users.Add(adminUser);
            db.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetService<ILoggerFactory>()?.CreateLogger("SeedData");
        logger?.LogError(ex, "An error occurred seeding the DB.");
    }
}

app.Run();

static string ComputeSha256Hash(string rawData)
{
    using (SHA256 sha256Hash = SHA256.Create())
    {
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
            builder.Append(bytes[i].ToString("x2"));
        return builder.ToString();
    }
}