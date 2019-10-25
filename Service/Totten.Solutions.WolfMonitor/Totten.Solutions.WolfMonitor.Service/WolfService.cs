using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using Totten.Solutions.WolfMonitor.Application.Base;
using Totten.Solutions.WolfMonitor.Application.Features.Agents;
using Totten.Solutions.WolfMonitor.Application.Ioc;

namespace Totten.Solutions.WolfMonitor.Service
{
    partial class WolfService : ServiceBase
    {
        private WindowsService _service;
        public WolfService()
        {
            InitializeComponent();
            _service = new WindowsService();
        }
        public void OnStartDebug(string[] args) => OnStart(args);
        protected override void OnStart(string[] args) => _service.Start();
        protected override void OnStop() => _service.Stop();
    }
}
