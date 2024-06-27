using TM.DailyTrackR.DataType.Enums;

namespace TM.DailyTrackR.DataType;

public class Status
{
    private StatusType type;

    public Status(StatusType type)
    {
        this.type = type;
    }

    public static List<Status> GetStatuses()
    {
        return new List<Status>()
        {
            new Status(StatusType.IN_PROGRESS),
            new Status(StatusType.ON_HOLD),
            new Status(StatusType.DONE)
        };
    }

    public int StatusId { get => (int) type; }

    public string StatusDescription
    {
        get
        {
            if (type == StatusType.IN_PROGRESS) return "In Progress";
            else if (type == StatusType.ON_HOLD) return "On Hold";
            else return "Done";
        }
    }
}