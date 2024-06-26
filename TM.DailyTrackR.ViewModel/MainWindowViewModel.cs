using Prism.Mvvm;
using System.Windows.Media;
using TM.DailyTrackR.Common;
using TM.DailyTrackR.DataType;
using TM.DailyTrackR.Logic;

namespace TM.DailyTrackR.ViewModel;

public sealed class MainWindowViewModel : BindableBase
{
    private readonly string currentUser;
    private readonly ActivityController activityController;
    private DateTime selectedDate;
    private List<DailyActivity> dailyActivities;
    private bool isLoading = false;

    public MainWindowViewModel(string currentUser)
    {
        this.currentUser = currentUser;
        this.activityController = LogicHelper.Instance.ActivityController;
        this.selectedDate = DateTime.Today;
    }

    public async Task InitializeAsync()
    {
        DailyActivities = await activityController.GetDailyActivitiesAsync(currentUser, selectedDate);
    }

    public DateTime CurrentDate { get => DateTime.Today; }

    public DateTime SelectedDate
    {
        get => selectedDate;
        set
        {
            if (isLoading) return;

            SetProperty(ref selectedDate, value);
            UpdateDailyActivitiesAsync();
        }
    }
    private async Task UpdateDailyActivitiesAsync()
    {
        IsLoading = true;
        DailyActivities = await activityController.GetDailyActivitiesAsync(currentUser, selectedDate);
        IsLoading = false;
    }

    public List<DailyActivity> DailyActivities
    {
        get => dailyActivities;
        set => SetProperty(ref dailyActivities, value);
    }

    public bool IsLoading
    {
        get => isLoading;
        set => SetProperty(ref isLoading, value);
    }
}
