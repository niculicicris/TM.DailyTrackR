namespace TM.DailyTrackR.DataType;

public class ActivityDisplay
{
    private int id;
    private int number;
    private string projectType;
    private string taskType;
    private string description;
    private string status;

    public ActivityDisplay(int id, int number, string projectType, string taskType, string description, string status)
    {
        this.id = id;
        this.number = number;
        this.projectType = projectType;
        this.taskType = taskType;
        this.description = description;
        this.status = status;
    }

    public int Number { get => number; }

    public string ProjectType { get => projectType; }

    public string TaskType { get => taskType; }

    public string Description { get => description; }

    public string Status { get => status; }
}