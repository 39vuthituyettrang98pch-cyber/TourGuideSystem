using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using UserMobile.Models;
using UserMobile.ViewModels;

namespace UserMobile.Views;

public partial class MapPage : ContentPage
{
    private MapViewModel ViewModel => BindingContext as MapViewModel ?? throw new InvalidOperationException();

    public MapPage()
    {
        InitializeComponent();
        BindingContext = App.Services.GetRequiredService<MapViewModel>();
        ViewModel.PlaceSelected += OnPlaceSelected;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (MapControl.Pins.Count == 0)
        {
            foreach (var place in ViewModel.Places)
            {
                var pin = new Pin
                {
                    Label = place.Title,
                    Address = place.Description,
                    Location = new Location(place.Latitude, place.Longitude),
                    Type = PinType.Place
                };

                pin.MarkerClicked += (s, e) =>
                {
                    e.HideInfoWindow = false;
                    ViewModel.SelectPlace(place);
                };

                MapControl.Pins.Add(pin);
            }

            if (ViewModel.Places.Count > 0)
            {
                var first = ViewModel.Places[0];
                MapControl.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(first.Latitude, first.Longitude), Distance.FromKilometers(1.2)));
            }
        }
    }

    private async void OnPlaceSelected(object? sender, PlaceItem place)
    {
        var detailPage = App.Services.GetRequiredService<PlaceDetailPage>();
        detailPage.LoadPlace(place);
        await Navigation.PushAsync(detailPage);
    }
}