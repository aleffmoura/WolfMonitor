using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Services
{
    public class LinuxService
    {
        public enum StatusLinux
        {
            Active,
            Inactive,
            Failed
        }

        static string CommandService(string serviceName, string command)
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

            return "Failed";
        }

        static string CommandSystemCtl(string serviceName, string command)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"sudo systemctl {command} {serviceName}\"",
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                }
            };
            process.Start();

            process.WaitForExit();

            var toReturn = process.StandardOutput.ReadToEnd();

            try
            {

                var rows = toReturn.Split("\n");

                var columns = rows[2].Split(" ", StringSplitOptions.RemoveEmptyEntries);

                var status = columns[1].Trim();

                if (status.Equals(StatusLinux.Active.ToString(), StringComparison.OrdinalIgnoreCase))
                    return StatusLinux.Active.ToString();
                if (status.Equals(StatusLinux.Inactive.ToString(), StringComparison.OrdinalIgnoreCase))
                    return StatusLinux.Inactive.ToString();

                return StatusLinux.Failed.ToString();
            }
            catch (Exception ex)
            {
                return StatusLinux.Failed.ToString();
            }
        }

        public static string GetStatus(string name, string displayName)
        {
            if (Microsoft.Extensions.Hosting.Systemd.SystemdHelpers.IsSystemdService())
                return CommandSystemCtl(name, "status");

            var realTimeNotSystemCtl = "Failed to enable APR_TCP_DEFER_ACCEPT";
            var returned = "";

            do
            {
                returned = CommandService(name, "status");

                if (Enum.TryParse(typeof(ServiceControllerStatus), returned, out object result))
                    return result.ToString();

                Thread.Sleep(500);
            } while (returned.Contains(realTimeNotSystemCtl));

            return "Falha";
        }

        private static bool VerifyAndReturnIfTrue(string status, string command)
        {
            if (Enum.TryParse(typeof(StatusLinux), status, out object result))
                return command.Equals("start") ?
                                result.ToString().Equals(StatusLinux.Active) :
                                result.ToString().Equals(StatusLinux.Inactive);

            return false;
        }
        public static bool Start(string name, string displayName)
        {
            if (Microsoft.Extensions.Hosting.Systemd.SystemdHelpers.IsSystemdService())
            {
                CommandSystemCtl(name, "start");

                var status = CommandSystemCtl(name, "status");

                return VerifyAndReturnIfTrue(status, "start");
            }
            else
            {
                CommandService(name, "start");
                return ServiceControllerStatus.Running.ToString().Equals(GetStatus(name, displayName), StringComparison.OrdinalIgnoreCase);
            }

        }

        public static bool Stop(string name, string displayName)
        {
            if (Microsoft.Extensions.Hosting.Systemd.SystemdHelpers.IsSystemdService())
            {
                CommandSystemCtl(name, "stop");

                var status = CommandSystemCtl(name, "status");

                return VerifyAndReturnIfTrue(status, "stop");
            }
            else
            {
                CommandService(name, "stop");
                return ServiceControllerStatus.Stopped.ToString().Equals(GetStatus(name, displayName), StringComparison.OrdinalIgnoreCase); ;
            }
        }
    }
}
