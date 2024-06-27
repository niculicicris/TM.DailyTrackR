using System.Data;
using System.Data.SqlClient;
using TM.DailyTrackR.DataType;
using TM.DailyTrackR.DataType.Constant;
using TM.DailyTrackR.DataType.Enums;

namespace TM.DailyTrackR.Logic;

public class ActivityController
{
    public List<DailyActivity> GetDailyActivities(string username, DateTime creationDate)
    {
        var dailyActivities = new List<DailyActivity>();
        var insertProcedureName = "tm.GetDailyActivities";

        using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
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
        ProjectTask task = new ProjectTask((TaskType) taskTypeId);
        string description = reader.GetString("description");
        int statusId = reader.GetInt32("status_id");
        Status status = new Status((StatusType) statusId);

        return new DailyActivity(id, number, projectType, task.TaskDescription, description, status.StatusDescription);
    }

    public void CreateActivity(ActivityDto activity)
    {
        var insertProcedureName = "tm.CreateActivity";

        using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
        {
            try
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(insertProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProjectTypeId", activity.ProjectTypeId);
                    command.Parameters.AddWithValue("@TaskTypeId", activity.Task.TaskId);
                    command.Parameters.AddWithValue("@Description", activity.Description);
                    command.Parameters.AddWithValue("@UserName", activity.Username);
                    command.Parameters.AddWithValue("@CreationDate", activity.CreationDate);
                    command.Parameters.AddWithValue("@StatusId", activity.Status.StatusId);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}