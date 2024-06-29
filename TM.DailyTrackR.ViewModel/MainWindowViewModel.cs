using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
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
    private DateTime selectedStartDate;
    private DateTime selectedEndDate;
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
        this.selectedStartDate = selectedDate;
        this.selectedEndDate = selectedDate;
        this.CreateActivityCommand = new DelegateCommand(CreateActivity, CanCreateActivity);
        this.ChangeStatusCommand = new DelegateCommand(ChangeStatus);
        this.DeleteCommand = new DelegateCommand(Delete);
        this.ExportCommand = new DelegateCommand(Export);
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

    public DateTime SelectedStartDate
    {
        get => selectedStartDate;
        set => SetProperty(ref selectedStartDate, value);
    }

    public DateTime SelectedEndDate
    {
        get => selectedEndDate;
        set => SetProperty(ref selectedEndDate, value);
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

    public DelegateCommand ChangeStatusCommand { get; }

    private void ChangeStatus()
    {
        if (isLoading) return;
        if (selectedDailyActivity == null) return;

        var changeViewModel = new ChangeStatusViewModel(this, selectedDailyActivity);
        ViewService.Instance.ShowDialog(changeViewModel);
    }

    public DelegateCommand DeleteCommand { get; }

    private void Delete()
    {
        if (isLoading) return;
        if (selectedDailyActivity == null) return;

        var deleteViewModel = new DeleteWindowViewModel(dailyActivities, overviewActivities, selectedDailyActivity.Id);
        ViewService.Instance.ShowDialog(deleteViewModel);
    }

    public DelegateCommand ExportCommand { get; }

    private void Export()
    {
        SaveFileDialog dialog = new SaveFileDialog();
        string startDate = selectedStartDate.ToString("dd.MM.yyyy");
        string endDate = selectedEndDate.ToString("dd.MM.yyyy");

        dialog.FileName = $"TeamActivity_{startDate}_{endDate}";
        dialog.DefaultExt = ".txt";
        dialog.Filter = "Text documents (.txt)|*.txt";

        Nullable<bool> result = dialog.ShowDialog();

        if (result == true)
        {
            string fileName = dialog.FileName;
            SaveExportFile(fileName, startDate, endDate);
        }
    }

    private void SaveExportFile(string filename, string startDate, string endDate)
    {
        var contentBuilder = new StringBuilder();
        var exportActivities = activityController.GetExportActivities(selectedStartDate, selectedEndDate);
        var projectGroups = exportActivities.GroupBy(activity => activity.ProjectType);

        contentBuilder.AppendLine($"Team Activity in the period {startDate} - {endDate}");
        contentBuilder.AppendLine();

        foreach (var projectGroup in projectGroups)
        {
            contentBuilder.AppendLine($"{projectGroup.Key}:");

            var taskGroups = projectGroup.GroupBy(activity => activity.TaskType);
            var newGroup = taskGroups.FirstOrDefault(group => group.Key != "Fix", null);
            var fixGroup = taskGroups.FirstOrDefault(group => group.Key == "Fix", null);

            if (newGroup != null)
            {
                newGroup.OrderBy(activity => activity.Status);

                foreach (var activity in newGroup)
                {
                    if (activity.Status == "Done")
                    {
                        contentBuilder.AppendLine($"  - {activity.Description}");
                    }
                    else
                    {
                        contentBuilder.AppendLine($"  - {activity.Description} - {activity.Status}");
                    }
                }
            }

            if (fixGroup != null)
            {
                fixGroup.OrderBy(activity => activity.Status);
                contentBuilder.AppendLine("  - Fixes:");

                foreach (var activity in fixGroup)
                {
                    if (activity.Status == "Done")
                    {
                        contentBuilder.AppendLine($"    - {activity.Description}");
                    }
                    else
                    {
                        contentBuilder.AppendLine($"    - {activity.Description} - {activity.Status}");
                    }
                }
            }

            contentBuilder.AppendLine();
        }

        File.WriteAllText(filename, contentBuilder.ToString());
    }
}
