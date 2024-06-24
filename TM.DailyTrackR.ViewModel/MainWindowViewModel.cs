using Prism.Mvvm;

namespace TM.DailyTrackR.ViewModel;

public sealed class MainWindowViewModel : BindableBase
{
    public MainWindowViewModel()
    {
       Calendar = new CalendarViewModel();
       Activity = new ActivityViewModel();
    }

    public CalendarViewModel Calendar { get; }

    public ActivityViewModel Activity { get; }
}
