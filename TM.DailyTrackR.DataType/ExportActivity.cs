namespace TM.DailyTrackR.DataType;

public class ExportActivity
{
    private string projectType;
    private string description;
    private string taskType;
    private string status;

    public ExportActivity(string projectType, string description, string taskType, string status)
    {
        this.projectType = projectType; ;
        this.description = description;
        this.taskType = taskType;
        this.status = status;
    }

    public string ProjectType { get => projectType; }

    public string Description { get => description; }

    public string TaskType { get => taskType; }

    public string Status { get => status; }
}