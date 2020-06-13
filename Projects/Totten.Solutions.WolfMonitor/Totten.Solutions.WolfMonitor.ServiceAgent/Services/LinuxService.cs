using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Services
{
    public class LinuxService
    {
        static string Command(string serviceName, string command)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"sudo service {serviceName} {command}\"",
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                }
            };
            process.Start();

            process.WaitForExit();

            var toReturn = process.StandardOutput.ReadToEnd();

            var errors = process.StandardOutput.ReadToEnd();

            if (errors.Contains("Failed to enable APR_TCP_DEFER_ACCEPT"))
                return errors;

            if (command == "start" && errors.Contains("sleep: cannot read realtime clock: Invalid argument"))
                return ServiceControllerStatus.StopPending.ToString();
            else if (command == "stop" && errors.Contains("sleep: cannot read realtime clock: Invalid argument"))
                return ServiceControllerStatus.StartPending.ToString();

            if (toReturn.Contains("is not running", StringComparison.OrdinalIgnoreCase))
                return ServiceControllerStatus.Stopped.ToString();
            if (toReturn.Contains("is running", StringComparison.OrdinalIgnoreCase))
                return ServiceControllerStatus.Running.ToString();

            return "falha";
        }

        public static string GetStatus(string name, string displayName)
        {
            var realtime = "Failed to enable APR_TCP_DEFER_ACCEPT";
            var returned = "";

            do
            {
                returned = Command(name, "status");

                if (Enum.TryParse(typeof(ServiceControllerStatus), returned, out object result))
                    return result.ToString();

                Thread.Sleep(1000);
            } while (returned.Contains(realtime));

            return "Falha";
        }
        public static bool Start(string name, string displayName)
        {
            Command(name, "start");

            return ServiceControllerStatus.Running.ToString().Equals(GetStatus(name, displayName), StringComparison.OrdinalIgnoreCase);
        }
        public static bool Stop(string name, string displayName)
        {
            Command(name, "stop");

            return ServiceControllerStatus.Stopped.ToString().Equals(GetStatus(name, displayName), StringComparison.OrdinalIgnoreCase); ;
        }
    }
}
