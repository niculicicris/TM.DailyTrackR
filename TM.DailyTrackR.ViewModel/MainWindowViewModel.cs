using Prism.Commands;
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
    private List<ProjectType> projectTypes;
    private List<DailyActivity> dailyActivities;
    private bool isLoading = false;

    public MainWindowViewModel(string currentUser)
    {
        this.currentUser = currentUser;
        this.activityController = LogicHelper.Instance.ActivityController;
        this.selectedDate = DateTime.Today;
        this.projectTypes = LogicHelper.Instance.ProjectTypeController.GetAllProjectTypes();
        this.dailyActivities = activityController.GetDailyActivities(currentUser, selectedDate);
        this.CreateActivityCommand = new DelegateCommand(CreateActivity, CanCreateActivity);
    }

    public string CurrentUser { get => currentUser; }

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
        DailyActivities = activityController.GetDailyActivities(currentUser, selectedDate);
        IsLoading = false;
    }

    public List<ProjectType> ProjectTypes { get => projectTypes; }

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

    public DelegateCommand CreateActivityCommand { get; }

    private void CreateActivity()
    {
        var createActivityViewModel = new CreateActivityWindowViewModel(this);
        ViewService.Instance.ShowDialog(createActivityViewModel);
    }

    private bool CanCreateActivity()
    {
        return !isLoading;
    }
}
