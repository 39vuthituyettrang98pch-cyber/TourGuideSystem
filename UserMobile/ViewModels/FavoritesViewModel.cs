using System.Collections.ObjectModel;
using System.Windows.Input;
using UserMobile.Models;

namespace UserMobile.ViewModels;

public class FavoritesViewModel : BaseViewModel
{
    private bool _isLoggedIn;

    public event EventHandler<PlaceItem>? PlaceSelected;

    public bool IsLoggedIn
    {
        get => _isLoggedIn;
        set => SetProperty(ref _isLoggedIn, value);
    }

    public ObservableCollection<PlaceItem> Favorites { get; } = new();
    public ICommand SelectFavoriteCommand { get; }

    public FavoritesViewModel()
    {
        SelectFavoriteCommand = new Command<PlaceItem>(OnFavoriteSelected);
        LoadSampleFavorites();
    }

    private void OnFavoriteSelected(PlaceItem place)
    {
        PlaceSelected?.Invoke(this, place);
    }

    private void LoadSampleFavorites()
    {
        Favorites.Add(new PlaceItem
        {
            Id = "place-walking",
            Title = "Phố đi bộ",
            Description = "Địa điểm yêu thích",
            Latitude = 21.0277,
            Longitude = 105.8342,
            HasNarration = true,
            ImageUrl = "walking_street.jpg",
            Introduction = "Phố đi bộ Hồ Hoàn Kiếm là khu vực đi bộ quanh hồ Hoàn Kiếm, được tổ chức vào các dịp cuối tuần với nhiều hoạt động văn hóa, nghệ thuật đường phố.",
            NarrationLanguages = new List<NarrationLanguage>
            {
                new() { Code = "vi", Name = "Tiếng Việt", NativeName = "Tiếng Việt", AudioUrl = "gia-nhu-anh-dung-lang-im.mp3" },
                new() { Code = "en", Name = "English", NativeName = "English", AudioUrl = "gia-nhu-anh-dung-lang-im.mp3" }
            }
        });
    }
}