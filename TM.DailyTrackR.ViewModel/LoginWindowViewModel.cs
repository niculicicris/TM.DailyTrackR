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
    private bool isLoading;

    public LoginWindowViewModel()
    {
        this.loginController = LogicHelper.Instance.LoginController;
        this.username = String.Empty;
        this.LoginCommand = new DelegateCommand<PasswordBox>(Login);
    }

    public string Username
    {
        get => username;
        set => SetProperty(ref username, value);
    }

    public DelegateCommand<PasswordBox> LoginCommand { get; }

    private void Login(PasswordBox passwordBox)
    {
        if (isLoading) return;
        var password = passwordBox.Password;

        if (!CanLogin(username, password))
        {
            ShowInvalidPasswordDialog();
            return;
        }

        LoginAsync(password);
    }

    private bool CanLogin(string username, string password)
    {
        if (username.Length == 0) return false;
        if (password.Length == 0) return false;

        return true;
    }

    private async Task LoginAsync(string password)
    {
        isLoading = true;

        var isLoggedIn = loginController.Login(username, password);
        if (isLoggedIn)
        {
            SwitchToMainWindowAsync();
        } else
        {
            ShowInvalidPasswordDialog();
        }

        isLoading = false;
    }

    private void SwitchToMainWindowAsync()
    {
        MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(username);
        ViewService.Instance.ShowWindow(mainWindowViewModel);
        ViewService.Instance.CloseWindow(this);
    }

    private void ShowInvalidPasswordDialog()
    {
        var errorMessage = "The entered username or password is not correct!";
        ViewService.Instance.ShowDialog(new ErrorWindowViewModel(errorMessage));
    }
}