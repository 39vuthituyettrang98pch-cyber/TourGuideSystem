using System.Collections.ObjectModel;
using System.Windows.Input;
using Plugin.Maui.Audio;
using UserMobile.Models;

namespace UserMobile.ViewModels;

public class PlaceDetailViewModel : BaseViewModel
{
    private PlaceItem? _place;
    private NarrationLanguage? _selectedLanguage;
    private bool _isPlaying;
    private bool _isPaused;
    private double _audioProgress;
    private double _audioDuration;
    private double _audioPosition;
    private string _currentTime = "00:00";
    private string _totalTime = "00:00";
    private double _playbackSpeed = 1.0;
    private bool _isPlayerVisible;
    private CancellationTokenSource? _positionCts;

    private IAudioPlayer? _audioPlayer;
    private readonly IAudioManager _audioManager;

    public ICommand PlayNarrationCommand { get; }
    public ICommand PauseNarrationCommand { get; }
    public ICommand StopNarrationCommand { get; }
    public ICommand SkipForwardCommand { get; }
    public ICommand SkipBackwardCommand { get; }
    public ICommand SelectLanguageCommand { get; }

    public PlaceItem? Place
    {
        get => _place;
        set => SetProperty(ref _place, value);
    }

    public NarrationLanguage? SelectedLanguage
    {
        get => _selectedLanguage;
        set
        {
            if (SetProperty(ref _selectedLanguage, value))
            {
                OnPropertyChanged(nameof(CanPlay));
            }
        }
    }

    public bool IsPlaying
    {
        get => _isPlaying;
        set => SetProperty(ref _isPlaying, value);
    }

    public bool IsPaused
    {
        get => _isPaused;
        set => SetProperty(ref _isPaused, value);
    }

    public double AudioProgress
    {
        get => _audioProgress;
        set
        {
            if (SetProperty(ref _audioProgress, value) && _audioPlayer != null && _audioDuration > 0)
            {
                var seekPosition = value * _audioDuration;
                _audioPlayer.Seek(seekPosition);
            }
        }
    }

    public double AudioDuration
    {
        get => _audioDuration;
        set => SetProperty(ref _audioDuration, value);
    }

    public double AudioPosition
    {
        get => _audioPosition;
        set => SetProperty(ref _audioPosition, value);
    }

    public string CurrentTime
    {
        get => _currentTime;
        set => SetProperty(ref _currentTime, value);
    }

    public string TotalTime
    {
        get => _totalTime;
        set => SetProperty(ref _totalTime, value);
    }

    public double PlaybackSpeed
    {
        get => _playbackSpeed;
        set
        {
            if (SetProperty(ref _playbackSpeed, value))
            {
                if (_audioPlayer != null)
                {
                    _audioPlayer.Speed = (float)value;
                }
            }
        }
    }

    public bool IsPlayerVisible
    {
        get => _isPlayerVisible;
        set => SetProperty(ref _isPlayerVisible, value);
    }

    public bool CanPlay => SelectedLanguage != null && Place?.HasNarration == true;

    public List<NarrationLanguage> NarrationLanguages => Place?.NarrationLanguages ?? new();

    public ObservableCollection<double> SpeedOptions { get; } = new() { 0.5, 1.0, 1.5, 2.0 };

    public PlaceDetailViewModel(IAudioManager audioManager)
    {
        _audioManager = audioManager;
        PlayNarrationCommand = new Command(async () => await PlayNarrationAsync());
        PauseNarrationCommand = new Command(PauseNarration);
        StopNarrationCommand = new Command(StopNarration);
        SkipForwardCommand = new Command(SkipForward);
        SkipBackwardCommand = new Command(SkipBackward);
        SelectLanguageCommand = new Command<NarrationLanguage>(OnLanguageSelected);
    }

    private void OnLanguageSelected(NarrationLanguage? language)
    {
        if (language != null)
        {
            SelectedLanguage = language;
            // If currently playing, stop and restart with new language
            if (_isPlaying || _isPaused)
            {
                StopNarration();
            }
        }
    }

    public void LoadPlace(PlaceItem place)
    {
        Place = place;
        OnPropertyChanged(nameof(NarrationLanguages));

        if (NarrationLanguages.Count > 0)
        {
            SelectedLanguage = NarrationLanguages[0];
        }

        // Calculate distance (simulated)
        Place.Distance = CalculateDistance(place.Latitude, place.Longitude);
    }

    private double CalculateDistance(double lat, double lon)
    {
        // Simulate distance calculation (would use actual GPS in real app)
        return Math.Round(new Random().NextDouble() * 2.0 + 0.1, 1);
    }

    public async Task PlayNarrationAsync()
    {
        if (_audioPlayer == null)
        {
            await InitializeAudioAsync();
        }

        if (_audioPlayer != null)
        {
            if (_isPaused)
            {
                _audioPlayer.Play();
                IsPaused = false;
                IsPlaying = true;
                StartPositionTracking();
            }
            else
            {
                _audioPlayer.Play();
                IsPlaying = true;
                IsPaused = false;
                IsPlayerVisible = true;
                StartPositionTracking();
            }
        }
    }

    public void PauseNarration()
    {
        if (_audioPlayer != null && _isPlaying)
        {
            _audioPlayer.Pause();
            IsPaused = true;
            IsPlaying = false;
            _positionCts?.Cancel();
        }
    }

    public void StopNarration()
    {
        _audioPlayer?.Stop();
        _audioPlayer?.Dispose();
        _audioPlayer = null;
        IsPlaying = false;
        IsPaused = false;
        IsPlayerVisible = false;
        AudioProgress = 0;
        AudioPosition = 0;
        CurrentTime = "00:00";
        _positionCts?.Cancel();
    }

    public void SkipForward()
    {
        if (_audioPlayer != null && _audioDuration > 0)
        {
            var position = _audioPlayer.CurrentPosition;
            var newPosition = Math.Min(position + 10, _audioDuration);
            _audioPlayer.Seek(newPosition);
        }
    }

    public void SkipBackward()
    {
        if (_audioPlayer != null)
        {
            var position = _audioPlayer.CurrentPosition;
            var newPosition = Math.Max(position - 10, 0);
            _audioPlayer.Seek(newPosition);
        }
    }

    public void SetSpeed(double speed)
    {
        PlaybackSpeed = speed;
    }

    private async Task InitializeAudioAsync()
    {
        try
        {
            // Load audio file from Resources/Raw
            var audioFileName = SelectedLanguage?.AudioUrl ?? "gia-nhu-anh-dung-lang-im.mp3";
            var stream = await FileSystem.OpenAppPackageFileAsync(audioFileName);
            _audioPlayer = _audioManager.CreatePlayer(stream);
            _audioPlayer.PlaybackEnded += OnPlaybackEnded;

            AudioDuration = _audioPlayer.Duration;
            TotalTime = FormatTime(AudioDuration);
            _audioPlayer.Speed = (float)PlaybackSpeed;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error initializing audio: {ex.Message}");
        }
    }

    private void OnPlaybackEnded(object? sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            IsPlaying = false;
            IsPaused = false;
            AudioProgress = 1.0;
            AudioPosition = AudioDuration;
            CurrentTime = TotalTime;
            _positionCts?.Cancel();
        });
    }

    private void StartPositionTracking()
    {
        _positionCts?.Cancel();
        _positionCts = new CancellationTokenSource();
        var token = _positionCts.Token;

        Task.Run(async () =>
        {
            while (!token.IsCancellationRequested && _audioPlayer != null)
            {
                if (_audioPlayer.IsPlaying)
                {
                    var position = _audioPlayer.CurrentPosition;
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        AudioPosition = position;
                        CurrentTime = FormatTime(position);
                        if (AudioDuration > 0)
                        {
                            AudioProgress = position / AudioDuration;
                        }
                    });
                }
                await Task.Delay(250, token);
            }
        }, token);
    }

    private static string FormatTime(double totalSeconds)
    {
        var time = TimeSpan.FromSeconds(totalSeconds);
        return time.TotalHours >= 1
            ? $"{time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}"
            : $"{time.Minutes:D2}:{time.Seconds:D2}";
    }
}