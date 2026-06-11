using Microsoft.Extensions.DependencyInjection;
using UserMobile.Models;
using UserMobile.ViewModels;

namespace UserMobile.Views;

public partial class RecentPlacesPage : ContentPage
{
    private RecentPlacesViewModel ViewModel => BindingContext as RecentPlacesViewModel ?? throw new InvalidOperationException();

    public RecentPlacesPage()
    {
        InitializeComponent();
        BindingContext = App.Services.GetRequiredService<RecentPlacesViewModel>();
        ViewModel.PlaceSelected += OnPlaceSelected;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    private async void OnPlaceSelected(object? sender, PlaceItem place)
    {
        var detailPage = App.Services.GetRequiredService<PlaceDetailPage>();
        detailPage.LoadPlace(place);
        await Navigation.PushAsync(detailPage);
    }
}