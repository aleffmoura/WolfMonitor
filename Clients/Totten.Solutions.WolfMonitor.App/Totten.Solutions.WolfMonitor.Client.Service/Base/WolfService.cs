using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using Totten.Solutions.WolfMonitor.Client.Appl.Features.Agents;
using Totten.Solutions.WolfMonitor.Client.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Client.Domain.Features.Monitorings;
using Totten.Solutions.WolfMonitor.Client.Service.Dtos;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Interfaces;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Client.Service.Base
{
    public class WolfService
    {
        private static readonly object _locker = new object();

        private Timer _timerLogin;
        private Timer _timerUpdateInfo;
        private Timer _timerPrincipal;

        private readonly int _oneMinute = 60000 / 2;

        private readonly IHelper _helper;
        private readonly IAgentService _agentService;
        private readonly AgentConfiguration _agentConfiguration;
        private Agent _agent;

        public WolfService(IAgentService agentService,
                           AgentConfiguration agentConfiguration,
                            IHelper helper)
        {
            _agentService = agentService;
            _agentConfiguration = agentConfiguration;
            _helper = helper;
        }

        public void Start()
        {
            _timerLogin = new Timer(_oneMinute);
            _timerLogin.Elapsed += TimerEventLogin;
            _timerLogin.Start();
        }

        public void Stop()
        {
            _timerLogin.Stop();
            _timerLogin.Elapsed -= TimerEventLogin;
            _timerLogin.Dispose();

            _timerUpdateInfo.Stop();
            _timerUpdateInfo.Elapsed -= TimerEventUpdate;
            _timerUpdateInfo.Dispose();
        }

        private void TimerEventLogin(object sender, ElapsedEventArgs e)
        {
            lock (_locker)
            {
                if (_agent != null)
                {
                    return;
                }

                Result<Exception, Agent> result = _agentService.GetInfo();

                result.OptionalSuccess.Match<Result<Exception, Agent>>(agent =>
                 {
                     _agent = agent;

                     _timerLogin.Stop();
                     _timerLogin.Elapsed -= TimerEventLogin;
                     _timerLogin.Dispose();
                     _timerLogin = null;

                     if (!_agent.Configured)
                     {
                         _timerUpdateInfo = new Timer(_oneMinute);
                         _timerUpdateInfo.Elapsed += TimerEventUpdate;
                         _timerUpdateInfo.Start();
                     }
                     else
                     {
                         _timerPrincipal = new Timer(_oneMinute);
                         _timerPrincipal.Elapsed += TimerEventService;
                         _timerPrincipal.Start();
                     }

                     return _agent;
                 }, () => result.Failure);
            }
        }

        private void TimerEventUpdate(object sender, ElapsedEventArgs e)
        {
            lock (_locker)
            {
                _agent = new Agent
                {
                    Name = Environment.MachineName,
                    HostAddress = _helper.GetMACAddress(),
                    HostName = _helper.GetHostName(),
                    LocalIp = _helper.GetLocalIpAddress(),
                    Configured = true
                };
                _agentService.Update(_agent);

                Result<Exception, Agent> infoCallback = _agentService.GetInfo();
                infoCallback.OptionalSuccess.Match<bool>(agent =>
                 {
                     _agent = agent;
                     _timerUpdateInfo.Stop();
                     _timerUpdateInfo.Elapsed -= TimerEventUpdate;
                     _timerUpdateInfo.Dispose();
                     _timerUpdateInfo = null;

                     _timerPrincipal = new Timer(_oneMinute);
                     _timerPrincipal.Elapsed += TimerEventService;
                     _timerPrincipal.Start();

                     return true;
                 }, () =>
                 {
                     //logar => infoCallback.Failure
                     return false;
                 });
            }
        }

        private void TimerEventService(object sender, ElapsedEventArgs e)
        {
            lock (_locker)
            {
                Task.Run(() =>
                {
                    Result<Exception, List<SystemService>> services = _agentService.GetServicesMonitoring();
                });
            }
        }
    }
}
