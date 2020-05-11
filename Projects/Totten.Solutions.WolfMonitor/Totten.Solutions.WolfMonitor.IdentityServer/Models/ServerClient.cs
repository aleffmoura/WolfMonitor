using System.Collections.Generic;

namespace Totten.Solutions.WolfMonitor.IdentityServer.Models
{
    public class ServerClient
    {
        public string Id { get; set; }
        public string Secret { get; set; }
        public string Name { get; set; }
        public List<string> Scopes { get; set; }
    }
}
