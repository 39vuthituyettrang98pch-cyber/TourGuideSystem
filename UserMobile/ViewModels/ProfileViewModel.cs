using UserMobile.Models;
using UserMobile.Services;

namespace UserMobile.ViewModels;

public class ProfileViewModel : BaseViewModel
{
    private readonly ILocalizationService _localizationService;
    private string _welcomeText = string.Empty;
    private bool _isLoggedIn;
    private UserProfile? _profile;

    public string WelcomeText
    {
        get => _welcomeText;
        set => SetProperty(ref _welcomeText, value);
    }

    public bool IsLoggedIn
    {
        get => _isLoggedIn;
        set => SetProperty(ref _isLoggedIn, value);
    }

    public UserProfile? Profile
    {
        get => _profile;
        set => SetProperty(ref _profile, value);
    }

    public ProfileViewModel(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
        WelcomeText = _localizationService.Translate("WelcomeTitle");
    }
}
