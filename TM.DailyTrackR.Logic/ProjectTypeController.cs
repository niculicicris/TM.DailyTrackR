using System.Data.SqlClient;
using System.Data;
using TM.DailyTrackR.DataType;
using TM.DailyTrackR.DataType.Constant;

namespace TM.DailyTrackR.Logic;

public class ProjectTypeController
{
    public List<ProjectType> GetAllProjectTypes()
    {
        var projectTypes = new List<ProjectType>();
        var insertProcedureName = "tm.GetAllProjectTypes";

        using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
        {
            try
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(insertProcedureName, connection))
                {

                    command.CommandType = CommandType.StoredProcedure;

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int id = reader.GetInt32("id");
                        string description = reader.GetString("description");

                        projectTypes.Add(new ProjectType(id, description));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        return projectTypes;
    }
}