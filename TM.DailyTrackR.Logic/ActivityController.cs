using System.Data;
using System.Data.SqlClient;
using TM.DailyTrackR.DataType;
using TM.DailyTrackR.DataType.Enums;

namespace TM.DailyTrackR.Logic;

public class ActivityController
{
    private string connectionString = @"Server=np:\\.\pipe\LOCALDB#9098F713\tsql\query;Database=TRACKER_DATA;Integrated Security=true;";

    public async Task<List<DailyActivity>> GetDailyActivitiesAsync(string username, DateTime creationDate)
    {
        var dailyActivities = new List<DailyActivity>();
        var insertProcedureName = "tm.GetDailyActivities";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(insertProcedureName, connection))
                {
                
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserName", username);
                    command.Parameters.AddWithValue("@SpecificDate", creationDate);
                
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var activity = ReadNextActivity(reader, dailyActivities.Count + 1);
                        dailyActivities.Add(activity);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        return dailyActivities;
    }

    private DailyActivity ReadNextActivity(SqlDataReader reader, int number)
    {
        int id = reader.GetInt32("id");
        string projectType = reader.GetString("project_type");
        int taskTypeId = reader.GetInt32("task_type_id");
        string taskType = GetTaskTypeName(taskTypeId);
        string description = reader.GetString("description");
        int statusId = reader.GetInt32("status_id");
        string status = GetStatusName(statusId);

        return new DailyActivity(id, number, projectType, taskType, description, status);
    }

    private string GetTaskTypeName(int taskTypeId)
    {
        if (taskTypeId == ((int) TaskType.FIX)) return "Fix";
        if (taskTypeId == ((int) TaskType.NEW)) return "New";

        return "Unknown";
    }

    private string GetStatusName(int statusId)
    {
        if (statusId == ((int) StatusType.IN_PROGRESS)) return "In Progress";
        if (statusId == ((int) StatusType.ON_HOLD)) return "On Hold";
        if (statusId == ((int) StatusType.DONE)) return "Done";

        return "Unknown";

    }
}