using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Controls;
using TM.DailyTrackR.Common;
using TM.DailyTrackR.Logic;

namespace TM.DailyTrackR.ViewModel;

public sealed class LoginWindowViewModel : BindableBase
{
    private readonly LoginController loginController;

    private string username;

    public LoginWindowViewModel()
    {
        loginController = LogicHelper.Instance.LoginController;
        username = String.Empty;
        LoginCommand = new DelegateCommand<PasswordBox>(Login);
    }

    public string Username
    {
        get => username;
        set => SetProperty(ref username, value);
    }

    public DelegateCommand<PasswordBox> LoginCommand { get; }

    public void Login(PasswordBox passwordBox)
    {
        var password = passwordBox.Password;

        if (!CanLogin(username, password))
        {
            ShowInvalidPasswordDialog();
            return;
        }

        if (!loginController.Login(username, password))
        {
            ShowInvalidPasswordDialog();
            return;
        }

        SwitchToMainWindow();
    }

    public bool CanLogin(String username, String password)
    {
        if (username.Length == 0) return false;
        if (password.Length == 0) return false;

        return true;
    }

    private void ShowInvalidPasswordDialog()
    {

    }

    private void SwitchToMainWindow()
    {
        ViewService.Instance.ShowWindow(new MainWindowViewModel());
        ViewService.Instance.CloseWindow(this);
    }
}