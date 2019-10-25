using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Timers;
using Totten.Solutions.WolfMonitor.Application.Features.Agents;
using Totten.Solutions.WolfMonitor.Application.Ioc;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Infra.Base;
using Totten.Solutions.WolfMonitor.Infra.Configurations;
using Totten.Solutions.WolfMonitor.Infra.Features.Agents;
using Totten.Solutions.WolfMonitor.Infra.Helpers;

namespace Totten.Solutions.WolfMonitor.Application.Base
{
    public class WindowsService
    {
        private int _minute = 60000;
        private Agent _agent;
        private Timer _service;
        private Timer _sendTime;
        private Timer _startAgent;
        private AgentService _agentService;
        private List<dynamic> _items;

        public WindowsService()
        {
            _service = new Timer(_minute);
            _service.Elapsed += Service;
        }
        public void Stop() => _service.Stop();
        public void Start() => _service.Start();
        private void Service(object sender, ElapsedEventArgs e)
        {
            var agentConfig = JsonConvert.DeserializeObject<AgentConfig>(ConfigurationManager.AppSettings["agentConfig"])
                          ?? throw new Exception("O Agente não foi configurado, por favor baixe a configuração no site.");
            Cfg.ApiUrl = agentConfig.ApiUrl;

            var user = new User
            {
                Name = agentConfig.UserName,
                Password = agentConfig.Password
            };
            _agent = new Agent
            {
                CompanyId = Guid.Parse(agentConfig.CompanyId),
                HostName = NetworkHelper.HostName,
                HostAddress = NetworkHelper.HostAddress,
                Name = Environment.MachineName,
                LocalIp = NetworkHelper.LocalIpAddress,
                Password = agentConfig.Password,
                UserName = agentConfig.UserName
            };

            _agentService = new AgentService(new AgentApiRepository(new CustomHttpCliente(Cfg.ApiUrl, ref user)));

            _service.Elapsed -= Service;
            _service.Stop();

            _startAgent = new Timer(_minute);
            _startAgent.Elapsed += StartAgent;
            _startAgent.Start();
        }
        private void StartAgent(object sender, ElapsedEventArgs e)
        {
            _agent.Id = _agentService.Add(_agent);

            if (!_agent.Id.Equals(Guid.Empty))
            {
                _startAgent.Elapsed -= StartAgent;
                _startAgent.Stop();

                _sendTime = new Timer(_minute);
                _sendTime.Elapsed += SendInfos;
                _sendTime.Start();
            }
        }
        private void SendInfos(object sender, ElapsedEventArgs e)
        {

        }
    }
}
