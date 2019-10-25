﻿namespace Totten.Solutions.WolfMonitor.Application.Features.Agents.ViewModels
{
    public class AgentViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LocalIp { get; set; }
        public string HostName { get; set; }
        public string HostAddress { get; set; }
        public string FirstConnection { get; set; }
        public string LastConnection { get; set; }
        public string LastUpload { get; set; }
    }
}
