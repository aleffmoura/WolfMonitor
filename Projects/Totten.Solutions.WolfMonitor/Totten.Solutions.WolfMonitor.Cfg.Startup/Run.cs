using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Helpers;

namespace Totten.Solutions.WolfMonitor.Cfg.Startup
{
    public static class Run
    {
        private static List<string> hosts = new List<string>();

        public static void Main<TStartup>(string[] args)
        {
            Helper helper = new Helper(null);

            var address = "192.168.0.102"//Environment.GetEnvironmentVariable("ADDRESS")
                            ?? helper.GetLocalIpAddress();

            var httpPort = Environment.GetEnvironmentVariable("PORT")
                            ?? helper.GenerateRandomPort().ToString();
            do
            {
                args = new string[] { $"http://{address}:{httpPort}" };

            } while (hosts.Contains(args[0]));

            hosts.Add(args[0]);

            IWebHost host = CreateWebHostBuilder<TStartup>(args).Build();
            host.Run();
        }
        private static IWebHostBuilder CreateWebHostBuilder<TStartup>(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                            .UseUrls(args)
                            .UseStartup(typeof(TStartup));
        }
    }
}
