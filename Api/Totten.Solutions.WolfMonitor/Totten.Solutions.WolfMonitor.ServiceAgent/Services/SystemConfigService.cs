using System.IO;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Services
{
    public class SystemConfigService
    {
        public static string GetCurrentValue(string path)
        {
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            return "config Not Found";
        }
    }
}
