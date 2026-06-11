using Microsoft.Extensions.DependencyInjection;
using UserMobile.Models;
using UserMobile.ViewModels;
using ZXing.Net.Maui;

namespace UserMobile.Views;

public partial class QrScannerPage : ContentPage
{
    private QrScannerViewModel ViewModel => BindingContext as QrScannerViewModel ?? throw new InvalidOperationException();

    public QrScannerPage()
    {
        InitializeComponent();
        BindingContext = App.Services.GetRequiredService<QrScannerViewModel>();
        ViewModel.PlaceScanned += OnPlaceScanned;
        
        BarcodeReader.BarcodesDetected += OnBarcodesDetected;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BarcodeReader.IsDetecting = true;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        BarcodeReader.IsDetecting = false;
    }

    private async void OnBarcodesDetected(object? sender, BarcodeDetectionEventArgs e)
    {
        if (e.Results?.Length > 0)
        {
            BarcodeReader.IsDetecting = false;
            var result = e.Results[0].Value;

            // Process the scanned code in the main thread
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (!string.IsNullOrEmpty(result))
                {
                    await ViewModel.ProcessScanResultAsync(result);
                }
                else
                {
                    BarcodeReader.IsDetecting = true;
                }
            });
        }
    }

    private async void OnPlaceScanned(object? sender, PlaceItem place)
    {
        var detailPage = App.Services.GetRequiredService<PlaceDetailPage>();
        detailPage.LoadPlace(place);
        await Navigation.PushAsync(detailPage);
    }
}