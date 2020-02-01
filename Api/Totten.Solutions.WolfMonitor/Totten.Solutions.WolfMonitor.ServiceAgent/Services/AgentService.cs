using System;
using System.Collections.Generic;
using System.Text;
using Totten.Solutions.WolfMonitor.ServiceAgent.Features.ItemAggregation;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Services
{
    public class AgentService
    {
        public object Login()
        {
            return true;
        }

        public List<Item> GetItems(object agent)
        {
            return null;
        }

        public void Send(Item item)
        {

        }
    }
}
