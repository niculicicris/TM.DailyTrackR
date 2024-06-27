using Prism.Commands;
using Prism.Mvvm;
using System.Reflection.Metadata.Ecma335;
using TM.DailyTrackR.Common;
using TM.DailyTrackR.DataType;
using TM.DailyTrackR.DataType.Enums;
using TM.DailyTrackR.Logic;

namespace TM.DailyTrackR.ViewModel;

public class CreateActivityWindowViewModel : BindableBase
{
    private readonly MainWindowViewModel mainViewModel;
    private readonly ActivityController activityController;
    private ProjectType selectedProjectType;
    private List<ProjectTask> tasks;
    private ProjectTask selectedTask;
    private string description;
    private List<Status> statuses;
    private Status selectedStatus;
    private DateTime selectedDate;
    private bool isLoading = false;

    public CreateActivityWindowViewModel(MainWindowViewModel mainViewModel)
    {
        this.mainViewModel = mainViewModel;
        this.activityController = LogicHelper.Instance.ActivityController;
        this.selectedProjectType = null;
        this.tasks = ProjectTask.GetTasks();
        this.selectedTask = null;
        this.description = String.Empty;
        this.statuses = Status.GetStatuses();
        this.SelectedStatus = null;
        this.selectedDate = mainViewModel.SelectedDate;
        this.CloseCommand = new DelegateCommand(Close);
        this.SaveCommand = new DelegateCommand(Save);
    }

    public MainWindowViewModel MainViewModel
    {
        get => mainViewModel;
    }

    public ProjectType SelectedProjectType
    {
        get => selectedProjectType;
        set => SetProperty(ref selectedProjectType, value);
    }

    public List<ProjectTask> Tasks { get => tasks; }

    public ProjectTask SelectedTask
    {
        get => selectedTask;
        set => SetProperty(ref selectedTask, value);
    }

    public string Description
    {
        get => description;
        set => SetProperty(ref description, value);
    }

    public List<Status> Statuses { get => statuses; }

    public Status SelectedStatus
    {
        get => selectedStatus;
        set => SetProperty(ref selectedStatus, value);
    }

    public DateTime SelectedDate
    {
        get => selectedDate;
        set => SetProperty(ref selectedDate, value);
    }

    public DelegateCommand CloseCommand { get; }

    public void Close()
    {
        ViewService.Instance.CloseWindow(this);
    }

    public DelegateCommand SaveCommand { get; }

    private void Save()
    {
        if (isLoading) return;

        if (!CanSave())
        {
            ShowEmptyFieldsError();
            return;
        }

        SaveAsync();
    }

    private bool CanSave()
    {
        if (SelectedProjectType == null) return false;
        if (selectedTask == null) return false;
        if (description == String.Empty) return false;
        if (selectedStatus == null) return false;

        return true;
    }

    private void ShowEmptyFieldsError()
    {
        var errorMessage = "Please enter data in all fields!";
        ViewService.Instance.ShowDialog(new ErrorWindowViewModel(errorMessage));
    }

    private async Task SaveAsync()
    {
        isLoading = true;

        ActivityDto activity = CreateActivityDto();

        activityController.CreateActivity(activity);
        mainViewModel.SelectedDate = selectedDate;
        ViewService.Instance.CloseWindow(this);

        isLoading = false;
    }

    private ActivityDto CreateActivityDto()
    {
        return new ActivityDto()
        {
            ProjectTypeId = selectedProjectType.Id,
            Task = selectedTask,
            Description = description,
            Username = mainViewModel.CurrentUser,
            CreationDate = selectedDate,
            Status = selectedStatus
        };
    }
}