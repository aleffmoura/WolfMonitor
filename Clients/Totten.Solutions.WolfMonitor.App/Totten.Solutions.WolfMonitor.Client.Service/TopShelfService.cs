using System;
using System.Collections.Generic;
using System.Text;
using Topshelf;

namespace Totten.Solutions.WolfMonitor.Client.Service
{
    public class TopShelfService : ServiceControl
    {
        public bool Start(HostControl hostControl)
        {
            Console.WriteLine("Started");
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            Console.WriteLine("Sttoped");
            return true;
        }
    }
}
