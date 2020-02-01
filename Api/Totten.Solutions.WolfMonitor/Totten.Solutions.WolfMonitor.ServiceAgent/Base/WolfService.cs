using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Timers;
using Totten.Solutions.WolfMonitor.ServiceAgent.Features.ItemAggregation;
using Totten.Solutions.WolfMonitor.ServiceAgent.Services;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Base
{
    public class WolfService
    {
        private FileSystemWatcher _watcher;
        private double _timerTick = 1000;
        private Timer _timer;
        private AgentService _agentService;
        private object _agent;

        public WolfService(AgentService agentService)
        {
            _timer = new Timer(_timerTick);
            _agentService = agentService;

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
                MakeArchiveWatcher();
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
            catch(Exception ex)
            {
                CreateLogException(ex);
            }

        }

        public void MakeArchiveWatcher()
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
            if (_agent == null)
            {
                _agent = _agentService.Login();
                return;
            }
            List<Item> items = _agentService.GetItems(_agent);

            foreach (Item item in items)
            {
                var instance = item.Type.GetInstance(item);
                if (instance.ShouldBeMonitoring())
                {
                    if (instance.VerifyChanges())
                    {
                        GenerateFile(instance);
                    }
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
            string path = $"./Monitoring/Exceptions/{item?.Name}_{item?.MonitoredAt.Value}_{DateTime.Now}.mon";
            File.WriteAllText(path, JsonConvert.SerializeObject(ex));
        }

    }
}
