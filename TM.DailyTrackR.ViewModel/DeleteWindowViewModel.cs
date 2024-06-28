using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using TM.DailyTrackR.Common;
using TM.DailyTrackR.DataType;
using TM.DailyTrackR.Logic;

namespace TM.DailyTrackR.ViewModel;

public class DeleteWindowViewModel : BindableBase
{
    private readonly ActivityController activityController;
    private ObservableCollection<DailyActivity> dailyActivities;
    private ObservableCollection<OverviewActivity> overviewActivities;
    private int activityId;

    public DeleteWindowViewModel(ObservableCollection<DailyActivity> dailyActivities, ObservableCollection<OverviewActivity> overviewActivities, int activityId)
    {
        this.activityController = LogicHelper.Instance.ActivityController;
        this.dailyActivities = dailyActivities;
        this.overviewActivities = overviewActivities;
        this.activityId = activityId;
        this.CloseCommand = new DelegateCommand(Close);
        this.DeleteCommand = new DelegateCommand(Delete);
    }

    public DelegateCommand CloseCommand { get; }

    public DelegateCommand DeleteCommand { get; }

    public void Close()
    {
        ViewService.Instance.CloseWindow(this);
    }

    public void Delete()
    {
        var dailyActivity = dailyActivities.First(activity => activity.Id == activityId);
        var overviewActivity = overviewActivities.First(activity => activity.Id == activityId);

        if (dailyActivity != null) dailyActivities.Remove(dailyActivity);
        if (overviewActivity != null) overviewActivities.Remove(overviewActivity);

        activityController.DeleteActivity(activityId);
        ViewService.Instance.CloseWindow(this);
    }
}