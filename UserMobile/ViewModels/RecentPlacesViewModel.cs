using System.Collections.ObjectModel;
using System.Windows.Input;
using UserMobile.Models;

namespace UserMobile.ViewModels;

public class RecentPlacesViewModel : BaseViewModel
{
    public event EventHandler<PlaceItem>? PlaceSelected;

    public ObservableCollection<PlaceItem> RecentPlaces { get; } = new();
    public ICommand SelectPlaceCommand { get; }

    public RecentPlacesViewModel()
    {
        SelectPlaceCommand = new Command<PlaceItem>(OnPlaceSelected);
        LoadSamplePlaces();
    }

    private void OnPlaceSelected(PlaceItem place)
    {
        PlaceSelected?.Invoke(this, place);
    }

    private void LoadSamplePlaces()
    {
        RecentPlaces.Add(new PlaceItem
        {
            Id = "place-church",
            Title = "Nhà thờ",
            Description = "Địa điểm có thuyết minh",
            Latitude = 21.0290,
            Longitude = 105.8530,
            HasNarration = true,
            ImageUrl = "church.jpg",
            Introduction = "Nhà thờ Lớn Hà Nội là một trong những nhà thờ lâu đời nhất tại Hà Nội, được xây dựng theo phong cách kiến trúc Gothic với mái vòm cao và cửa kính màu đặc trưng.",
            NarrationLanguages = new List<NarrationLanguage>
            {
                new() { Code = "vi", Name = "Tiếng Việt", NativeName = "Tiếng Việt", AudioUrl = "gia-nhu-anh-dung-lang-im.mp3" },
                new() { Code = "en", Name = "English", NativeName = "English", AudioUrl = "gia-nhu-anh-dung-lang-im.mp3" }
            }
        });

        RecentPlaces.Add(new PlaceItem
        {
            Id = "place-square",
            Title = "Quảng trường",
            Description = "Địa điểm phổ biến",
            Latitude = 21.0280,
            Longitude = 105.8520,
            HasNarration = true,
            ImageUrl = "square.jpg",
            Introduction = "Quảng trường Ba Đình là quảng trường lớn nhất Việt Nam, nằm trước Lăng Chủ tịch Hồ Chí Minh. Đây là nơi diễn ra nhiều sự kiện chính trị, văn hóa quan trọng của đất nước.",
            NarrationLanguages = new List<NarrationLanguage>
            {
                new() { Code = "vi", Name = "Tiếng Việt", NativeName = "Tiếng Việt", AudioUrl = "gia-nhu-anh-dung-lang-im.mp3" },
                new() { Code = "en", Name = "English", NativeName = "English", AudioUrl = "gia-nhu-anh-dung-lang-im.mp3" },
                new() { Code = "fr", Name = "French", NativeName = "Français", AudioUrl = "gia-nhu-anh-dung-lang-im.mp3" }
            }
        });
    }
}