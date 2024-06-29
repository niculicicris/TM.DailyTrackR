using Prism.Commands;
using Prism.Mvvm;
using System.Reflection.Metadata.Ecma335;
using TM.DailyTrackR.Common;
using TM.DailyTrackR.DataType;
using TM.DailyTrackR.DataType.Enums;
using TM.DailyTrackR.Logic;

namespace TM.DailyTrackR.ViewModel;

public class ChangeStatusViewModel : BindableBase
{
    private readonly MainWindowViewModel mainViewModel;
    private readonly DailyActivity activity;
    private readonly ActivityController activityController;
    private List<Status> statuses;
    private Status selectedStatus;
    private bool isLoading = false;

    public ChangeStatusViewModel(MainWindowViewModel mainViewModel, DailyActivity activity)
    {
        this.mainViewModel = mainViewModel;
        this.activity = activity;
        this.activityController = LogicHelper.Instance.ActivityController;
        this.statuses = Status.GetStatuses();
        this.selectedStatus = statuses.First(status => status.StatusDescription == activity.Status);
        this.CloseCommand = new DelegateCommand(Close);
        this.SaveCommand = new DelegateCommand(Save);
    }

    public MainWindowViewModel MainViewModel
    {
        get => mainViewModel;
    }

    public List<Status> Statuses { get => statuses; }

    public Status SelectedStatus
    {
        get => selectedStatus;
        set => SetProperty(ref selectedStatus, value);
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
        SaveAsync();
    }

    private async Task SaveAsync()
    {
        isLoading = true;

        activityController.ChangeActivityStatus(activity.Id, selectedStatus.StatusId);
        mainViewModel.SelectedDate = mainViewModel.SelectedDate;
        ViewService.Instance.CloseWindow(this);

        isLoading = false;
    }
}