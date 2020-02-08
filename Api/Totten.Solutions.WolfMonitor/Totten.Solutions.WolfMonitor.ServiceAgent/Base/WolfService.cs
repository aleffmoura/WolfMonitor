using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Interfaces;
using Totten.Solutions.WolfMonitor.ServiceAgent.Features.ItemAggregation;
using Totten.Solutions.WolfMonitor.ServiceAgent.Services;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Base
{
    public class WolfService
    {
        public object _locker = new object();
        private readonly int _timerTick = 1000;
        private bool _started = false;

        private readonly IHelper _helper;
        private AgentService _agentService;
        private Agent _agent;

        public WolfService(AgentService agentService, IHelper helper)
        {
            _agentService = agentService;
            _helper = helper;
        }

        public void CreateDirs()
        {
            var pathServices = $"./Monitoring";

            if (!Directory.Exists(pathServices))
            {
                Directory.CreateDirectory(pathServices);
                Directory.CreateDirectory($"./Monitoring/Exceptions");
            }
        }

        public void Start()
        {
            _started = true;
            Task.Factory.StartNew(() =>
            {

                CreateDirs();
                Service();
            });
        }

        public void Stop() => _started = false;

        private void CreateLogException(Exception ex, Item item = null)
        {
            try
            {
                string path = $"./Monitoring/Exceptions/{item?.Name}_{item?.MonitoredAt.Value.ToString("ddMMyyyyhhmmss")}_{DateTime.Now.ToString("ddMMyyyyhhmmss")}.mon";
                File.WriteAllText(path, JsonConvert.SerializeObject(ex));
            }
            catch { }
        }

        private void GenerateFile(Item item)
        {
            try
            {
                string path = $"./Monitoring/{item.Name}_{item.Type.ToString()}_{item.MonitoredAt.Value.ToString("ddMMyyyyhhmmss")}.mon";

                File.WriteAllText(path, JsonConvert.SerializeObject(item));
            }
            catch (Exception ex)
            {
                CreateLogException(ex, item);
            }
        }

        private void Service()
        {
            while (_started)
            {
                try
                {
                    if (_agent == null)
                    {
                        var agentCallback = _agentService.Login();

                        if (agentCallback.IsSuccess)
                            _agent = agentCallback.Success;
                        else
                            CreateLogException(agentCallback.Failure);
                        continue;
                    }

                    if (!_agent.Configured)
                    {
                        _agent.MachineName = Environment.MachineName;
                        _agent.HostAddress = _helper.GetMACAddress();
                        _agent.HostName = _helper.GetHostName();
                        _agent.LocalIp = _helper.GetLocalIpAddress();

                        _agentService.Update(_agent);

                        var agentCallback = _agentService.GetInfo();

                        if (agentCallback.IsSuccess)
                            _agent = agentCallback.Success;
                        else
                            CreateLogException(agentCallback.Failure);
                        continue;
                    }

                    var itemsCallback = _agentService.GetItems();

                    if (itemsCallback.IsSuccess)
                        for (int i = 0; i < itemsCallback.Success.Items.Count; i++)
                        {
                            var instance = itemsCallback.Success.Items[i].Type.GetInstance(itemsCallback.Success.Items[i]);

                            if (instance.ShouldBeMonitoring())
                                if (instance.VerifyChanges())
                                {
                                    GenerateFile(instance);
                                    _agentService.Send(instance);
                                }

                            itemsCallback.Success.Items[i] = instance;
                        }
                    else
                        CreateLogException(itemsCallback.Failure);
                }
                catch (Exception ex)
                {
                    CreateLogException(ex);
                }

                Thread.Sleep(_timerTick);
            }

        }

    }
}
