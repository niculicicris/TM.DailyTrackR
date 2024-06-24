using Prism.Mvvm;

namespace TM.DailyTrackR.ViewModel;

public class CalendarViewModel : BindableBase
{
    private DateTime selectedDate;

    public CalendarViewModel()
    {
        selectedDate = DateTime.Now;
    }

    public DateTime CurrentDate { get => DateTime.Now; }
    public DateTime SelectedDate
    {
        get => selectedDate;
        set => SetProperty(ref selectedDate, value);
    }
}