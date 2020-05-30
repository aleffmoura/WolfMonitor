using System;
using System.IO;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Services
{
    public class SystemArchivesService
    {
        public static string GetCurrentValue(string path)
        {
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            return "Archive Not Found";
        }

        public static void ChangeValue(string path, string newValue)
        {
            if (File.Exists(path))
                File.WriteAllText(path, newValue);
        }

    }
}
