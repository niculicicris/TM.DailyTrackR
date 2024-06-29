namespace TM.DailyTrackR.DataType.Constant;

public class Connection
{
    private Connection() { }

    public static string ConnectionString
    {
        get => @"Server=np:\\.\pipe\LOCALDB#1AE6F52B\tsql\query;Database=TRACKER_DATA;Integrated Security=true;";
    }
}