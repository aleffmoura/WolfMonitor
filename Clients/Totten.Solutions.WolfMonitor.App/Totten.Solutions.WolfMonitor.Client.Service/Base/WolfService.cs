using Newtonsoft.Json;
using System.Timers;
using Totten.Solutions.WolfMonitor.Client.Appl.Features.Agents;
using Totten.Solutions.WolfMonitor.Client.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Client.Service.Dtos;

namespace Totten.Solutions.WolfMonitor.Client.Service.Base
{
    public class WolfService
    {
        static object _locker = new object();
        private int _oneMinute = 1000;
        private Timer _loginTimer;
        private readonly IAgentService _agentService;
        private AgentConfiguration _agentConfiguration;
        private Agent _agent;

        public WolfService(IAgentService agentService, AgentConfiguration agentConfiguration)
        {
            _agentService = agentService;
            _agentConfiguration = agentConfiguration;
        }

        private void Login(object sender, ElapsedEventArgs e)
        {
            lock (_locker)
            {
                _agent = _agentService.GetInfo().Success;

                if (_agent != null)
                {
                    _loginTimer.Stop();
                }
            }
        }

        public void Start()
        {
            _loginTimer = new Timer(_oneMinute);
            _loginTimer.Elapsed += Login;
            _loginTimer.Start();
        }

        public void Stop()
        {

        }
    }
}
