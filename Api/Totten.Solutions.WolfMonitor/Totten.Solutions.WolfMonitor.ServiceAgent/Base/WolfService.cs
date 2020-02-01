using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Timers;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Interfaces;
using Totten.Solutions.WolfMonitor.ServiceAgent.Features.ItemAggregation;
using Totten.Solutions.WolfMonitor.ServiceAgent.Services;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Base
{
    public class WolfService
    {
        public static object _locker = new object();
        private readonly double _timerTick = 500;

        private readonly IHelper _helper;
        private AgentService _agentService;
        private FileSystemWatcher _watcher;
        private Agent _agent;
        private Timer _timer;

        public WolfService(AgentService agentService, IHelper helper)
        {
            _agentService = agentService;
            _helper = helper;
            _timer = new Timer(_timerTick);

            CreateDirs();
        }

        public void CreateDirs()
        {
            var pathServices = $"./Monitoring";

            if (!Directory.Exists(pathServices))
            {
                Directory.CreateDirectory(pathServices);
                Directory.CreateDirectory($"./Monitoring/Exceptions");
            }

            if (_watcher == null)
            {
                MakeconfigWatcher();
            }
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

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            try
            {
                var json = File.ReadAllText(e.FullPath);
                var item = JsonConvert.DeserializeObject<Item>(json);
                _agentService.Send(item);
            }
            catch (Exception ex)
            {
                CreateLogException(ex);
            }

        }

        public void MakeconfigWatcher()
        {
            _watcher = new FileSystemWatcher();
            _watcher.Path = $"./Monitoring";
            _watcher.NotifyFilter = NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.FileName
                                 | NotifyFilters.DirectoryName;

            _watcher.Filter = "*.mon";
            _watcher.Created += OnFileCreated;
            _watcher.EnableRaisingEvents = true;
        }

        private void Tick(object sender, ElapsedEventArgs e)
        {
            lock (_locker)
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
                        return;
                    }

                    if (!_agent.Configured)
                    {
                        _agent.Name = Environment.MachineName;
                        _agent.HostAddress = _helper.GetMACAddress();
                        _agent.HostName = _helper.GetHostName();
                        _agent.LocalIp = _helper.GetLocalIpAddress();

                        _agentService.Update(_agent);

                        var agentCallback = _agentService.GetInfo();

                        if (agentCallback.IsSuccess)
                            _agent = agentCallback.Success;
                        else
                            CreateLogException(agentCallback.Failure);
                        return;
                    }

                    var itemsCallback = _agentService.GetItems();

                    if (itemsCallback.IsSuccess)
                    {
                        foreach (Item item in itemsCallback.Success.Items)
                        {
                            var instance = item.Type.GetInstance(item);

                            if (instance.ShouldBeMonitoring())
                                if (instance.VerifyChanges())
                                    GenerateFile(instance);
                        }
                    }
                    else
                        CreateLogException(itemsCallback.Failure);
                }
                catch (Exception ex)
                {
                    CreateLogException(ex);
                }
            }
        }

        private void GenerateFile(Item item)
        {
            try
            {
                CreateDirs();
                string path = $"./Monitoring/{item.Name}_{item.MonitoredAt.Value}_{item.Type.ToString()}.mon";

                File.WriteAllText(path, JsonConvert.SerializeObject(item));
            }
            catch (Exception ex)
            {
                CreateLogException(ex, item);
            }
        }

        private void CreateLogException(Exception ex, Item item = null)
        {
            try
            {
                string path = $"./Monitoring/Exceptions/{item?.Name}_{item?.MonitoredAt.Value}_{DateTime.Now}.mon";
                File.WriteAllText(path, JsonConvert.SerializeObject(ex));
            }
            catch { }
        }

    }
}
