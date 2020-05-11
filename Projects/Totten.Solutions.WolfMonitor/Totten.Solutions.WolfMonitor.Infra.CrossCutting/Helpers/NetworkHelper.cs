using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Totten.Solutions.WolfMonitor.Infra.CrossCutting.Helpers
{

    public static class NetworkHelper
    {
        public static int RandomPort()
            => new Random(Guid.NewGuid().GetHashCode()).Next(16000, 17000);

        public static string LocalIpAddress()
            => Dns.GetHostEntry(Dns.GetHostName())
                .AddressList
                    .First(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();

    }
}
