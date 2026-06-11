using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;
using UserMobile.Services;
using UserMobile.Views;
using ZXing.Net.Maui.Controls;

namespace UserMobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiMaps()
            .UseBarcodeReader()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<ILocalizationService, LocalizationService>();
        builder.Services.AddSingleton<ILocalStorageService, LocalStorageService>();
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddHttpClient<IApiService, ApiService>(client =>
        {
            client.BaseAddress = new Uri("https://api.example.com/");
        });

        // Audio
        builder.Services.AddSingleton(AudioManager.Current);

        // ViewModels
        builder.Services.AddSingleton<ViewModels.LanguageSelectionViewModel>();
        builder.Services.AddSingleton<ViewModels.MapViewModel>();
        builder.Services.AddSingleton<ViewModels.RecentPlacesViewModel>();
        builder.Services.AddSingleton<ViewModels.QrScannerViewModel>();
        builder.Services.AddSingleton<ViewModels.FavoritesViewModel>();
        builder.Services.AddSingleton<ViewModels.ProfileViewModel>();
        builder.Services.AddSingleton<ViewModels.LoginViewModel>();
        builder.Services.AddSingleton<ViewModels.PlaceDetailViewModel>();

        // Pages
        builder.Services.AddTransient<PlaceDetailPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var mauiApp = builder.Build();
        App.InitializeServices(mauiApp.Services);
        return mauiApp;
    }
}
