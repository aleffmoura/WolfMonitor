using System;
using System.Collections.Generic;
using System.Text;

namespace Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Monitorings.NovaPasta
{
    public class SystemServiceViewModel
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string LastStatus { get; set; } = "stop";
    }
}
