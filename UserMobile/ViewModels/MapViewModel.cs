using System.Collections.ObjectModel;
using System.Windows.Input;
using UserMobile.Models;

namespace UserMobile.ViewModels;

public class MapViewModel : BaseViewModel
{
    private bool _isLoading;

    public event EventHandler<PlaceItem>? PlaceSelected;

    public ObservableCollection<PlaceItem> Places { get; } = new();
    public ICommand SelectPlaceCommand { get; }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public MapViewModel()
    {
        SelectPlaceCommand = new Command<PlaceItem>(SelectPlace);
        LoadSamplePlaces();
    }

    public void SelectPlace(PlaceItem place)
    {
        PlaceSelected?.Invoke(this, place);
    }

    private void LoadSamplePlaces()
    {
        Places.Add(new PlaceItem
        {
            Id = "place-opera",
            Title = "Nhà hát lớn",
            Description = "Hướng dẫn thuyết minh tự động",
            Latitude = 21.0285,
            Longitude = 105.8542,
            HasNarration = true,
            ImageUrl = "opera_house.jpg",
            Introduction = "Nhà hát Lớn Hà Nội là một công trình kiến trúc tiêu biểu của Pháp tại Hà Nội, được xây dựng từ năm 1901 đến 1911. Đây là nơi diễn ra các sự kiện văn hóa nghệ thuật quan trọng của thủ đô.",
            NarrationLanguages = new List<NarrationLanguage>
            {
                new() { Code = "vi", Name = "Tiếng Việt", NativeName = "Tiếng Việt", AudioUrl = "gia-nhu-anh-dung-lang-im.mp3" },
                new() { Code = "en", Name = "English", NativeName = "English", AudioUrl = "gia-nhu-anh-dung-lang-im.mp3" },
                new() { Code = "ko", Name = "Korean", NativeName = "한국어", AudioUrl = "gia-nhu-anh-dung-lang-im.mp3" },
                new() { Code = "zh", Name = "Chinese", NativeName = "中文", AudioUrl = "gia-nhu-anh-dung-lang-im.mp3" }
            }
        });

        Places.Add(new PlaceItem
        {
            Id = "place-hoankiem",
            Title = "Hồ Hoàn Kiếm",
            Description = "Địa điểm tham quan nổi tiếng",
            Latitude = 21.0288,
            Longitude = 105.8525,
            HasNarration = true,
            ImageUrl = "hoan_kiem_lake.jpg",
            Introduction = "Hồ Hoàn Kiếm, còn gọi là Hồ Gươm, là một hồ nước ngọt tự nhiên nằm ở trung tâm thành phố Hà Nội. Hồ gắn liền với truyền thuyết vua Lê Lợi trả gươm thần cho Rùa thần.",
            NarrationLanguages = new List<NarrationLanguage>
            {
                new() { Code = "vi", Name = "Tiếng Việt", NativeName = "Tiếng Việt", AudioUrl = "gia-nhu-anh-dung-lang-im.mp3" },
                new() { Code = "en", Name = "English", NativeName = "English", AudioUrl = "gia-nhu-anh-dung-lang-im.mp3" },
                new() { Code = "ja", Name = "Japanese", NativeName = "日本語", AudioUrl = "gia-nhu-anh-dung-lang-im.mp3" }
            }
        });
    }
}