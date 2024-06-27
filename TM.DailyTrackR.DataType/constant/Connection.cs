namespace TM.DailyTrackR.DataType.Constant;

public class Connection
{
    private Connection() { }

    public static string ConnectionString
    {
        get => @"Server=np:\\.\pipe\LOCALDB#9AF58288\tsql\query;Database=TRACKER_DATA;Integrated Security=true;";
    }
}