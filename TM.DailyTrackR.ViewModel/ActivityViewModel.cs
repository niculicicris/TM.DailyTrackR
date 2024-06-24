using Prism.Mvvm;
using TM.DailyTrackR.DataType;

namespace TM.DailyTrackR.ViewModel;

public class ActivityViewModel : BindableBase
{
    private List<ActivityDisplay> activitiesDisplay;

    public ActivityViewModel()
    {
        activitiesDisplay = new List<ActivityDisplay>();

        for (int i = 0; i < 10; ++i)
        {
            activitiesDisplay.Add(new ActivityDisplay(1, 1, "Administration", "Fix", "this is a very basic test", "In Progress"));
            activitiesDisplay.Add(new ActivityDisplay(4, 2, "Marketing", "New", "test 2", "On Hold"));
        }
    }

    public List<ActivityDisplay> Activities
    {
        get => activitiesDisplay;
    }
}