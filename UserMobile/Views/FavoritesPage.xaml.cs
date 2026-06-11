using Microsoft.Extensions.DependencyInjection;
using UserMobile.Models;
using UserMobile.ViewModels;

namespace UserMobile.Views;

public partial class FavoritesPage : ContentPage
{
    private FavoritesViewModel ViewModel => BindingContext as FavoritesViewModel ?? throw new InvalidOperationException();

    public FavoritesPage()
    {
        InitializeComponent();
        BindingContext = App.Services.GetRequiredService<FavoritesViewModel>();
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