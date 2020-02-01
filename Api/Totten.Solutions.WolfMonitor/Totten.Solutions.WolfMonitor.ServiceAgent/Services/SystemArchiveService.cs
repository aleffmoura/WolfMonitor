using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Services
{
    public class SystemArchiveService
    {
        public static string GetCurrentValue(string path)
        {
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            return "Archive Not Found";
        }
    }
}
