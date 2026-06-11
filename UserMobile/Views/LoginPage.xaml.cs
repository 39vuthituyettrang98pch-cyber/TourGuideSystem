using Microsoft.Extensions.DependencyInjection;
using UserMobile.ViewModels;

namespace UserMobile.Views;

public partial class LoginPage : ContentPage
{
    private LoginViewModel ViewModel => BindingContext as LoginViewModel ?? throw new InvalidOperationException();

    public LoginPage()
    {
        InitializeComponent();
        BindingContext = App.Services.GetRequiredService<LoginViewModel>();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var result = await ViewModel.LoginAsync();
        if (result.IsSuccess)
        {
            await Shell.Current.GoToAsync("//MapPage");
        }
    }
}
