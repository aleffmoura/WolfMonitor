using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Totten.Solutions.WolfMonitor.ServiceAgent.Services;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Base
{
    public class WolfService
    {
        private double _timerTick = 1000;
        private Timer _timer;
        private AgentService _agentService;
        private object _agent;

        public WolfService(AgentService agentService)
        {
            _timer = new Timer(_timerTick);
            _agentService = agentService;
        }
        public void Start()
        {
            _timer.Enabled = true;
            _timer.Elapsed += Tick;
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
            _timer.Enabled = false;
            _timer.Elapsed -= Tick;
        }
        private void Tick(object sender, ElapsedEventArgs e)
        {
            if (_agent == null)
            {
                _agent = _agentService.Login();
                return;
            }


        }
    }
}
