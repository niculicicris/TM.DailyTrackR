using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using TM.DailyTrackR.DataType;
using TM.DailyTrackR.DataType.Constant;
using TM.DailyTrackR.DataType.Enums;

namespace TM.DailyTrackR.Logic;

public class ActivityController
{
    public List<DailyActivity> GetDailyActivities(string username, DateTime creationDate)
    {
        var dailyActivities = new List<DailyActivity>();
        var getProcedureName = "tm.GetDailyActivities";

        using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
        {
            try
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(getProcedureName, connection))
                {
                
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserName", username);
                    command.Parameters.AddWithValue("@SpecificDate", creationDate);
                
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var activity = ReadNextDailyActivity(reader, dailyActivities.Count + 1);
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

    private DailyActivity ReadNextDailyActivity(SqlDataReader reader, int number)
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
            }
        }
    }

    public void DeleteActivity(int id)
    {
        var deleteProcedureName = "tm.DeleteActivity";

        using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
        {
            try
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(deleteProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }

    public List<OverviewActivity> GetOverviewActivities(DateTime creationDate)
    {
        var overviewActivities = new List<OverviewActivity>();
        var getProcedureName = "tm.GetOverviewActivities";

        using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
        {
            try
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(getProcedureName, connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SpecificDate", creationDate);

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var activity = ReadNextOverviewActivity(reader, overviewActivities.Count + 1);
                        overviewActivities.Add(activity);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        return overviewActivities;
    }

    private OverviewActivity ReadNextOverviewActivity(SqlDataReader reader, int number)
    {
        int id = reader.GetInt32("id");
        string projectType = reader.GetString("project_type");
        string description = reader.GetString("description");
        int statusId = reader.GetInt32("status_id");
        Status status = new Status((StatusType) statusId);
        string user = reader.GetString("user");

        return new OverviewActivity(id, number, projectType, description, status.StatusDescription, user);
    }

    public List<ExportActivity> GetExportActivities(DateTime startDate, DateTime endDate)
    {
        var exportActivities = new List<ExportActivity>();
        var getProcedureName = "tm.GetExportActivities";

        using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
        {
            try
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(getProcedureName, connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var activity = ReadNextExportActivity(reader);
                        exportActivities.Add(activity);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        return exportActivities;
    }

    private ExportActivity ReadNextExportActivity(SqlDataReader reader)
    {
        string projectType = reader.GetString("project_type");
        string description = reader.GetString("description");
        int taskTypeId = reader.GetInt32("task_type_id");
        ProjectTask taskType = new ProjectTask((TaskType) taskTypeId);
        int statusId = reader.GetInt32("status_id");
        Status status = new Status((StatusType)statusId);

        return new ExportActivity(projectType, description, taskType.TaskDescription, status.StatusDescription);
    }

    public void ChangeActivityStatus(int id, int statusId)
    {
        var updateProcedureName = "tm.ChangeActivityStatus";

        using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
        {
            try
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(updateProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@StatusId", statusId);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}