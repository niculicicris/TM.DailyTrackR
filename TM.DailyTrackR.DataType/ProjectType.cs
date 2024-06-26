namespace TM.DailyTrackR.DataType;

public class ProjectType
{
    private int id;
    private string description;

    public ProjectType(int id, string description)
    {
        this.id = id;
        this.description = description;
    }

    public int Id { get => id; }

    public string Description { get => description; }
}