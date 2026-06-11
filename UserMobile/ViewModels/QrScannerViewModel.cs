using System.Windows.Input;
using UserMobile.Models;

namespace UserMobile.ViewModels;

public class QrScannerViewModel : BaseViewModel
{
    private string _scanResult = "Chưa có kết quả";
    private bool _isScanned;

    public event EventHandler<PlaceItem>? PlaceScanned;

    public string ScanResult
    {
        get => _scanResult;
        set => SetProperty(ref _scanResult, value);
    }

    public bool IsScanned
    {
        get => _isScanned;
        set => SetProperty(ref _isScanned, value);
    }

    public ICommand ResetScanCommand { get; }

    public QrScannerViewModel()
    {
        ResetScanCommand = new Command(ResetScan);
    }

    public async Task ProcessScanResultAsync(string qrData)
    {
        IsScanned = true;
        ScanResult = $"Đã quét: {qrData}";

        // Try to find a place that matches the QR code data
        var place = FindPlaceByQrData(qrData);
        if (place != null)
        {
            ScanResult = $"Tìm thấy: {place.Title}";
            PlaceScanned?.Invoke(this, place);
        }
        else
        {
            ScanResult = $"Không tìm thấy địa điểm: {qrData}";
            // Reset scanner after a delay
            await Task.Delay(3000);
            ResetScan();
        }
    }

    public void ResetScan()
    {
        ScanResult = "Chưa có kết quả";
        IsScanned = false;
    }

    private PlaceItem? FindPlaceByQrData(string qrData)
    {
        // Simulate looking up a place by QR data
        // In a real app, this would query an API or local database
        var allPlaces = GetAllSamplePlaces();
        return allPlaces.Find(p =>
            p.Id == qrData ||
            p.Title.Contains(qrData, StringComparison.OrdinalIgnoreCase) ||
            qrData.Contains(p.Id, StringComparison.OrdinalIgnoreCase));
    }

    private List<PlaceItem> GetAllSamplePlaces()
    {
        return new List<PlaceItem>
        {
            new()
            {
                Id = "place-opera",
                Title = "Nhà hát lớn",
                Description = "Hướng dẫn thuyết minh tự động",
                Latitude = 21.0285,
                Longitude = 105.8542,
                HasNarration = true,
                ImageUrl = "opera_house.jpg",
                Introduction = "Nhà hát Lớn Hà Nội là một công trình kiến trúc tiêu biểu của Pháp tại Hà Nội, được xây dựng từ năm 1901 đến 1911.",
                NarrationLanguages = new List<NarrationLanguage>
                {
                    new() { Code = "vi", Name = "Tiếng Việt", NativeName = "Tiếng Việt", AudioUrl = "gia-nhu-anh-dung-lang-im.mp3" },
                    new() { Code = "en", Name = "English", NativeName = "English", AudioUrl = "gia-nhu-anh-dung-lang-im.mp3" }
                }
            },
            new()
            {
                Id = "place-hoankiem",
                Title = "Hồ Hoàn Kiếm",
                Description = "Địa điểm tham quan nổi tiếng",
                Latitude = 21.0288,
                Longitude = 105.8525,
                HasNarration = true,
                ImageUrl = "hoan_kiem_lake.jpg",
                Introduction = "Hồ Hoàn Kiếm, còn gọi là Hồ Gươm, là một hồ nước ngọt tự nhiên nằm ở trung tâm thành phố Hà Nội.",
                NarrationLanguages = new List<NarrationLanguage>
                {
                    new() { Code = "vi", Name = "Tiếng Việt", NativeName = "Tiếng Việt", AudioUrl = "gia-nhu-anh-dung-lang-im.mp3" },
                    new() { Code = "en", Name = "English", NativeName = "English", AudioUrl = "gia-nhu-anh-dung-lang-im.mp3" }
                }
            },
            new()
            {
                Id = "place-church",
                Title = "Nhà thờ Lớn",
                Description = "Nhà thờ cổ kính",
                Latitude = 21.0290,
                Longitude = 105.8530,
                HasNarration = true,
                ImageUrl = "church.jpg",
                Introduction = "Nhà thờ Lớn Hà Nội là một trong những nhà thờ lâu đời nhất tại Hà Nội.",
                NarrationLanguages = new List<NarrationLanguage>
                {
                    new() { Code = "vi", Name = "Tiếng Việt", NativeName = "Tiếng Việt", AudioUrl = "gia-nhu-anh-dung-lang-im.mp3" },
                    new() { Code = "en", Name = "English", NativeName = "English", AudioUrl = "gia-nhu-anh-dung-lang-im.mp3" }
                }
            }
        };
    }
}