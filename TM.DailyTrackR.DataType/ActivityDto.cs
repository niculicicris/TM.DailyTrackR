using TM.DailyTrackR.DataType.Enums;

namespace TM.DailyTrackR.DataType;

public class ActivityDto
{
    private int id;
    private int projectTypeId;
    private TaskType taskType;
    private string description;
    private string username;
    private DateTime creationDate = DateTime.Now;
    private StatusType status;

    public int Id { get => id; set => id = value; }

    public int ProjectTypeId { get => projectTypeId; set => projectTypeId = value; }

    public TaskType TaskType { get => taskType; set => taskType = value; }

    public string Description { get => description; set => description = value; }
    
    public string Username { get => username; set => username = value; }

    public DateTime CreationDate { get => creationDate; set => creationDate = value; }

    public StatusType Status { get => status; set => status = value; }
}