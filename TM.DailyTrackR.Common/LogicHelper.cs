using TM.DailyTrackR.Logic;

namespace TM.DailyTrackR.Common
{
    public sealed class LogicHelper
    {
        private static readonly Lazy<LogicHelper> Lazy = new Lazy<LogicHelper>(() => new LogicHelper(), isThreadSafe: true);
        
        private LogicHelper()
        {
            ExampleController = new ExampleController();
        }

        public static LogicHelper Instance { get { return Lazy.Value; } }

        public ExampleController ExampleController { get; }
    }
}
