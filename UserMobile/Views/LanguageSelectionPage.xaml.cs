using Microsoft.Extensions.DependencyInjection;
using UserMobile.ViewModels;

namespace UserMobile.Views;

public partial class LanguageSelectionPage : ContentPage
{
    private LanguageSelectionViewModel ViewModel => BindingContext as LanguageSelectionViewModel ?? throw new InvalidOperationException();

    public LanguageSelectionPage()
    {
        InitializeComponent();
        BindingContext = App.Services.GetRequiredService<LanguageSelectionViewModel>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.InitializeAsync();
    }

    private async void OnContinueClicked(object sender, EventArgs e)
    {
        if (await ViewModel.SaveLanguageAsync())
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
