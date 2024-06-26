using TM.DailyTrackR.DataType;

namespace TM.DailyTrackR.Logic;

public class ProjectTypeController
{
    public async Task<List<ProjectType>> GetAllProjectTypes()
    {
        return new List<ProjectType>()
        {
            new ProjectType(1, "Administrative"),
            new ProjectType(1, "Marketing")
        };
    }
}