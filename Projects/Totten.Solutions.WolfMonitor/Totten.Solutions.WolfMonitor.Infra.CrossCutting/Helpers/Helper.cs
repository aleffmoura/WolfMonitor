using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Interfaces;

namespace Totten.Solutions.WolfMonitor.Infra.CrossCutting.Helpers
{
    public class Helper : IHelper
    {
        private static List<string> _ports;

        private readonly IConfigurationRoot _configuration;
        
        public Helper(IConfigurationRoot configuration) => this._configuration = configuration;

        public string GetLocalIpAddress() => NetworkHelper.LocalIpAddress();
        public int GenerateRandomPort()
        {
            _ports = _ports ?? new List<string>();

            int port;
            do
            {
                port = NetworkHelper.RandomPort();
            } while (_ports.Contains(port.ToString()));
            _ports.Add(port.ToString());
            return port;
        }

        public string GetServiceName()
        {
            var assemblyName = AssemblyHelper.GetAssemblyName();
            var serviceName = assemblyName.Split(".").Last();
            return serviceName;
        }
        public string GetMACAddress()
            => NetworkInterface.GetAllNetworkInterfaces()
                                                  .ToList()
                                                  .FirstOrDefault(x => !string.IsNullOrEmpty(x.GetPhysicalAddress().ToString()))
                                                  .GetPhysicalAddress()
                                                  .ToString();
        public string GetAssemblyVersion() => AssemblyHelper.AssemblyVersion();
        public SigningCredentials GetSigningCredentials() => JwtHelper.GetSigningCredentials();
        public string GetConfiguration(string key) => this._configuration[key];
        public string GetAssemblyName() => AssemblyHelper.GetAssemblyName();
        public string GetHostName() => Dns.GetHostEntry(Environment.MachineName).HostName;
    }
}
