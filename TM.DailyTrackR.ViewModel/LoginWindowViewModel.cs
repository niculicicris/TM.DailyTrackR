using Prism.Commands;
using Prism.Mvvm;
using TM.DailyTrackR.Common;

namespace TM.DailyTrackR.ViewModel;

public class LoginWindowViewModel : BindableBase
{
    private string username;
    private string password;
    private DelegateCommand loginCommand;

    public LoginWindowViewModel()
    {
        username = String.Empty;
        password = String.Empty;
        loginCommand = new DelegateCommand(Login, CanLogin);
    }

    public string Username
    {
        get => username;
        set => SetProperty(ref username, value);
    }

    public string Password
    {
        get => password;
        set => SetProperty(ref password, value);
    }

    public DelegateCommand LoginCommand
    {
        get => loginCommand;
    }

    public void Login()
    {

    }

    public bool CanLogin() => true;
}