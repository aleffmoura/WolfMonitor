using Totten.Solutions.WolfMonitor.Client.Domain.Features.Users;

namespace Totten.Solutions.WolfMonitor.Client.Service.Dtos
{
    public class AgentConfiguration
    {
        public string UrlApi { get; set; }
        public string Company { get; set; }
        public User User { get; set; }
    }
}
