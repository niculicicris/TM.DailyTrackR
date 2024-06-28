namespace TM.DailyTrackR.DataType;

public class OverviewActivity
{
    private int id;
    private int number;
    private string projectType;
    private string description;
    private string status;
    private string user;

    public OverviewActivity(int id, int number, string projectType, string description, string status, string user)
    {
        this.id = id;
        this.number = number;
        this.projectType = projectType;
        this.description = description;
        this.status = status;
        this.user = user;
    }

    public int Id { get => id; }

    public int Number { get => number; }

    public string ProjectType { get => projectType; }

    public string Description { get => description; }

    public string Status { get => status; }

    public string User { get => user; }
}