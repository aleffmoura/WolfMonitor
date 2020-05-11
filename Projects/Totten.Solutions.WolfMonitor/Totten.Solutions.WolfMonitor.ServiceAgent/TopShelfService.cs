using System;
using System.Collections.Generic;
using System.Text;
using Topshelf;
using Totten.Solutions.WolfMonitor.ServiceAgent.Base;

namespace Totten.Solutions.WolfMonitor.ServiceAgent
{
    public class TopShelfService : ServiceControl
    {
        private WolfService _service;

        public TopShelfService(WolfService service)
        {
            _service = service;
        }
        public bool Start(HostControl hostControl)
        {
            Console.WriteLine("Started");
            _service.Start();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            Console.WriteLine("Sttoped");
            _service.Stop();
            return true;
        }
    }
}
