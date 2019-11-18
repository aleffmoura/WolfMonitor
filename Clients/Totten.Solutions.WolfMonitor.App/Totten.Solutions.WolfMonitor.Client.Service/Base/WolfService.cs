using System;
using System.Timers;
using Totten.Solutions.WolfMonitor.Client.Appl.Features.Agents;
using Totten.Solutions.WolfMonitor.Client.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Client.Service.Dtos;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Client.Service.Base
{
    public class WolfService
    {
        private readonly object _locker = new object();

        private Timer _loginTimer;
        private Timer _updateInfo;

        private readonly int _oneMinute = 1000;

        private readonly IAgentService _agentService;
        private readonly AgentConfiguration _agentConfiguration;
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
                Result<Exception, Agent> result = _agentService.GetInfo();

                result.OptionalSuccess.Match<Result<Exception, Agent>>(agent =>
                 {
                     _agent = agent;

                     _loginTimer.Stop();
                     _loginTimer.Dispose();

                     if (!_agent.Configured)
                     {
                         _updateInfo = new Timer(_oneMinute);
                         _updateInfo.Elapsed += Update;
                         _updateInfo.Start();
                     }

                     return _agent;
                 }, () => result.Failure);
            }
        }

        private void Update(object sender, ElapsedEventArgs e)
        {
            lock (_locker)
            {
                _agent = new Agent
                {
                    Name = "",
                    HostAddress = "",
                    HostName = "",
                    LocalIp = "",
                    Configured = true
                };
                _agentService.Update(_agent);
                _agentService.GetInfo().OptionalSuccess.Match<bool>(agent =>
                 {
                     _agent = agent;
                     _updateInfo.Stop();
                     _updateInfo.Dispose();
                     return true;
                 }, () =>
                 {

                     return false;
                 });
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
