using System.Windows;
using TM.DailyTrackR.Common;
using TM.DailyTrackR.View;
using TM.DailyTrackR.ViewModel;

namespace TM.DailyTrackR;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ViewService.Instance.RegisterView(typeof(LoginWindowViewModel), typeof(LoginWindow));
        ViewService.Instance.RegisterView(typeof(MainWindowViewModel), typeof(MainWindow));
        ViewService.Instance.RegisterView(typeof(ErrorWindowViewModel), typeof(ErrorWindow));
        ViewService.Instance.RegisterView(typeof(CreateActivityWindowViewModel), typeof(CreateActivityWindow));

        ViewService.Instance.ShowWindow(new LoginWindowViewModel());
    }
}