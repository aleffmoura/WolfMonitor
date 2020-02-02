using Totten.Solutions.WolfMonitor.Agents;
using Totten.Solutions.WolfMonitor.Cfg.Startup;

namespace Totten.Solutions.WolfMonitor.Monitoring
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Run.Main<Startup>(args);
        }
    }
}
