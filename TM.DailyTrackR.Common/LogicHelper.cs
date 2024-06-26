using TM.DailyTrackR.Logic;

namespace TM.DailyTrackR.Common;

public sealed class LogicHelper
{
    private static readonly Lazy<LogicHelper> Lazy = new Lazy<LogicHelper>(() => new LogicHelper(), isThreadSafe: true);

    private LogicHelper()
    {
        LoginController = new LoginController();
        ActivityController = new ActivityController();
        ProjectTypeController = new ProjectTypeController();
    }

    public static LogicHelper Instance { get { return Lazy.Value; } }

    public LoginController LoginController { get; }

    public ActivityController ActivityController { get; }

    public ProjectTypeController ProjectTypeController { get; }
}