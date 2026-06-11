using System.Threading.Tasks;
using System.Windows.Input;
using UserMobile.Models;
using UserMobile.Services;

namespace UserMobile.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
    private string _email = string.Empty;
    private string _password = string.Empty;
    private bool _isBusy;
    private string _message = string.Empty;
    private bool _hasMessage;

    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            SetProperty(ref _isBusy, value);
            ((Command)LoginCommand).ChangeCanExecute();
        }
    }

    public string Message
    {
        get => _message;
        set
        {
            SetProperty(ref _message, value);
            HasMessage = !string.IsNullOrWhiteSpace(value);
        }
    }

    public bool HasMessage
    {
        get => _hasMessage;
        set => SetProperty(ref _hasMessage, value);
    }

    public ICommand LoginCommand { get; }

    public LoginViewModel(IAuthService authService)
    {
        _authService = authService;
        LoginCommand = new Command(async () => await LoginAsync(), CanExecuteLogin);
    }

    public async Task<AuthResult> LoginAsync()
    {
        IsBusy = true;
        ((Command)LoginCommand).ChangeCanExecute();
        var result = await _authService.LoginAsync(Email, Password);
        Message = result.Message;
        IsBusy = false;
        ((Command)LoginCommand).ChangeCanExecute();
        return result;
    }

    private bool CanExecuteLogin()
    {
        return !IsBusy;
    }
}
