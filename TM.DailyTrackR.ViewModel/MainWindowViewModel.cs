using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    private ObservableCollection<DailyActivity> dailyActivities;
    private DailyActivity selectedDailyActivity;
    private ObservableCollection<OverviewActivity> overviewActivities;
    private bool isLoading = false;

    public MainWindowViewModel(string currentUser)
    {
        this.currentUser = currentUser;
        this.activityController = LogicHelper.Instance.ActivityController;
        this.selectedDate = DateTime.Today;
        this.projectTypes = LogicHelper.Instance.ProjectTypeController.GetAllProjectTypes();
        this.dailyActivities = new ObservableCollection<DailyActivity>();
        this.dailyActivities.AddRange(activityController.GetDailyActivities(currentUser, selectedDate));
        this.selectedDailyActivity = null;
        this.overviewActivities = new ObservableCollection<OverviewActivity>();
        this.overviewActivities.AddRange(activityController.GetOverviewActivities(selectedDate));
        this.CreateActivityCommand = new DelegateCommand(CreateActivity, CanCreateActivity);
        this.DeleteCommand = new DelegateCommand(Delete);
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
            UpdateActivitiesAsync();
        }
    }
    private async Task UpdateActivitiesAsync()
    {
        IsLoading = true;

        DailyActivities.Clear();
        DailyActivities.AddRange(activityController.GetDailyActivities(currentUser, selectedDate));
        OverviewActivities.Clear();
        OverviewActivities.AddRange(activityController.GetOverviewActivities(selectedDate));
        
        IsLoading = false;
    }

    public List<ProjectType> ProjectTypes { get => projectTypes; }

    public ObservableCollection<DailyActivity> DailyActivities
    {
        get => dailyActivities;
        set => SetProperty(ref dailyActivities, value);
    }

    public DailyActivity SelectedDailyActivity
    {
        get => selectedDailyActivity;
        set => SetProperty(ref selectedDailyActivity, value);
    }

    public ObservableCollection<OverviewActivity> OverviewActivities 
    { 
        get => overviewActivities; 
        set => SetProperty(ref overviewActivities, value); 
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

    public DelegateCommand DeleteCommand { get; }

    private void Delete()
    {
        if (isLoading) return;
        if (selectedDailyActivity == null) return;

        var deleteViewModel = new DeleteWindowViewModel(dailyActivities, overviewActivities, selectedDailyActivity.Id);
        ViewService.Instance.ShowDialog(deleteViewModel);
    }
}
