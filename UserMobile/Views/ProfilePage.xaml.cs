using Microsoft.Extensions.DependencyInjection;
using UserMobile.ViewModels;

namespace UserMobile.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
        BindingContext = App.Services.GetRequiredService<ProfileViewModel>();
    }
}
