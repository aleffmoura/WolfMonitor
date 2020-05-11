using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Totten.Solutions.WolfMonitor.SignalR.Hubs.Users
{
    public class UserConnected
    {
        public string Id { get; set; }
        public List<string> ConnectIds { get; set; }
    }
}
