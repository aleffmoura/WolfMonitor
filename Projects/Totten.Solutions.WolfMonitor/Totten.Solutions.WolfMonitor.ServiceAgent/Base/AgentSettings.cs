using System;
using Totten.Solutions.WolfMonitor.ServiceAgent.Infra.Base;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Base
{
    public class AgentSettings
    {
        public string urlApi { get; set; }
        public string Company { get; set; }
        public int IntervalForSearchItensSeconds { get; set; }
        public int NextMonitoringItemIfGenerateFileInMinutes { get; set; }
        public int RetrySendIfFailInHours { get; set; }
        public string PathFilesIfFailSend { get; set; }
        public string PathFilesExceptions { get; set; }
        public bool ReadItemsMonitoringByArchive { get; set; }
        
        public UserLogin User { get; set; }
    }
}
