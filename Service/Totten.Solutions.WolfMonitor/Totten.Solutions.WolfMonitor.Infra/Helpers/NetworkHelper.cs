using System;
using System.Linq;
using System.Net;

namespace Totten.Solutions.WolfMonitor.Infra.Helpers
{
    public static class NetworkHelper
    {
        public static int RandomPort()
        {
            return new Random(Guid.NewGuid().GetHashCode()).Next(16000, 17000);
        }

        public static string LocalIpAddress
            => Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).First().ToString();
        public static string HostName
            => Dns.GetHostName();
        public static string HostAddress
            => Dns.GetHostAddresses(Dns.GetHostName()).Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).First().ToString();
    }
}
