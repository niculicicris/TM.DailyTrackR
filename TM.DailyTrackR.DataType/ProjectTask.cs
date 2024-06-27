using TM.DailyTrackR.DataType.Enums;

namespace TM.DailyTrackR.DataType;

public class ProjectTask
{
    public TaskType type;

    public ProjectTask(TaskType type)
    {
        this.type = type;
    }

    public static List<ProjectTask> GetTasks()
    {
        return new List<ProjectTask>()
        {
            new ProjectTask(TaskType.NEW),
            new ProjectTask(TaskType.FIX)
        };
    }

    public int TaskId { get => (int) type; }

    public string TaskDescription
    {
        get
        {
            if (type == TaskType.NEW) return "New";
            else return "Fix";
        }
    }
}