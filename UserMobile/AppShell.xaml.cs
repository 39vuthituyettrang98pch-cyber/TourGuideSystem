using Microsoft.Extensions.DependencyInjection;
using UserMobile.Services;
using UserMobile.Views;

namespace UserMobile;

public partial class AppShell : Shell
{
    private readonly ILocalizationService _localizationService;

    public AppShell()
    {
        InitializeComponent();
        _localizationService = App.Services?.GetRequiredService<ILocalizationService>() ?? throw new InvalidOperationException("Services not initialized.");
        Routing.RegisterRoute(nameof(LanguageSelectionPage), typeof(LanguageSelectionPage));
        Routing.RegisterRoute(nameof(PlaceDetailPage), typeof(PlaceDetailPage));
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var savedLanguage = await _localizationService.GetSavedLanguageAsync();
        if (savedLanguage is null)
        {
            await Shell.Current.GoToAsync(nameof(LanguageSelectionPage));
        }
    }
}
