using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Interfaces;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.RabbitMQService;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Totten.Solutions.WolfMonitor.ServiceAgent.Features.ItemAggregation;
using Totten.Solutions.WolfMonitor.ServiceAgent.Infra.Base;
using Totten.Solutions.WolfMonitor.ServiceAgent.Infra.Features.Monitorings.VOs;
using Totten.Solutions.WolfMonitor.ServiceAgent.Services;
using Timer = System.Timers.Timer;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Base
{
    public class WolfService
    {
        private bool _started = false;
        private int _whileCycle = 1000;

        private Agent _agent;
        private IHelper _helper;
        private AgentService _agentService;
        private Timer _sendFiles;
        private AgentSettings _agentSettings;
        private Rabbit _rabbitMQ;
        private CancellationTokenSource _cancellationToken;
        private Result<Exception, PageResult<Item>> _items;

        public WolfService(AgentSettings agentSettings, AgentService agentService, IHelper helper)
        {
            _agentService = agentService;
            _helper = helper;
            _agentSettings = agentSettings;
            _cancellationToken = new CancellationTokenSource();
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

        private void Tick(object sender, ElapsedEventArgs e)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(_agentSettings.PathFilesIfFailSend);

            if (directoryInfo.Exists)
            {
                foreach (FileInfo fileInfo in directoryInfo.GetFiles() ?? new FileInfo[0])
                {
                    try
                    {
                        var item = JsonConvert.DeserializeObject<Item>(File.ReadAllText(fileInfo.FullName));
                        if (_agentService.Send(item).IsSuccess)
                            fileInfo.Delete();
                    }
                    catch (Exception ex)
                    {
                        fileInfo.Delete();
                        GenerateLogException(ex);
                    }
                }
            }
        }

        private void ConfigureTimerSendFiles(bool initial = true)
        {
            if (initial)
            {
                _sendFiles = new Timer(_agentSettings.RetrySendIfFailInHours * 3.6e+6);
                _sendFiles.Elapsed += Tick;
                _sendFiles.Enabled = true;
                _sendFiles.Start();
                return;
            }
            _sendFiles.Elapsed -= Tick;
            _sendFiles.Enabled = false;
            _sendFiles.Stop();
            _sendFiles.Dispose();
        }

        public void Start()
        {
            _started = true;
            Task.Factory.StartNew(() =>
            {

                CreateDirs();
                ConfigureTimerSendFiles();
                Service();
            });
        }

        public void Stop()
        {
            _started = false;
            _cancellationToken.Cancel();
        }

        private void GenerateLogException(Exception ex, Item item = default)
        {
            try
            {
                var obj = new
                {
                    Exception = JsonConvert.SerializeObject(ex.Message),
                    Item = item
                };
                File.WriteAllText(Path.Combine(_agentSettings.PathFilesExceptions, $"{item?.Name}_{item?.MonitoredAt.Value.ToString("ddMMyyyyhhmmss")}_{DateTime.Now.ToString("ddMMyyyyhhmmss")}.mon"),
                                  JsonConvert.SerializeObject(obj, Formatting.Indented));
            }
            catch { }
        }

        private void GenerateFile(Item item)
        {
            try
            {
                File.WriteAllText(Path.Combine(_agentSettings.PathFilesIfFailSend, $"{item.Name}_{item.Type.ToString()}_{item.MonitoredAt.Value.ToString("ddMMyyyyhhmmss")}.mon"),
                                  JsonConvert.SerializeObject(item));
            }
            catch (Exception ex)
            {
                GenerateLogException(ex, item);
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
                            GenerateLogException(agentCallback.Failure);
                        continue;
                    }

                    if (!_agent.Configured)
                    {
                        AgentUpdateVO agent = new AgentUpdateVO();
                        agent.MachineName = Environment.MachineName;
                        agent.HostAddress = _helper.GetMACAddress();
                        agent.HostName = _helper.GetHostName();
                        agent.LocalIp = _helper.GetLocalIpAddress();

                        _agentService.Update(agent);

                        var agentCallback = _agentService.GetInfo();

                        if (agentCallback.IsSuccess)
                            _agent = agentCallback.Success;
                        else
                            GenerateLogException(agentCallback.Failure);
                        continue;
                    }

                    GetItems();

                    if (_rabbitMQ == null)
                    {
                        _rabbitMQ = new Rabbit(null, null);
                        Task.Run(() =>
                        {
                            _rabbitMQ.Receive(ReceivedMessage, _cancellationToken.Token, queue: _agent.Id.ToString());
                        }, _cancellationToken.Token);
                    }
                }
                catch (Exception ex)
                {
                    GenerateLogException(ex);
                }

                Thread.Sleep(_whileCycle);
            }

        }

        private void GetItems()
        {
            if (_agentSettings.ReadItemsMonitoringByArchive)
            {
                _items = Result<Exception, PageResult<Item>>.Of(new PageResult<Item>());
                _items.Success.Items = new List<Item>();
            }
            else
                _items = _agentService.GetItems();

            if (_items.IsSuccess)
                VerifyChanges(_items);
            else
                GenerateLogException(_items.Failure);
        }

        private void VerifyChanges(Result<Exception, PageResult<Item>> itemsCallback)
        {
            for (int i = 0; i < itemsCallback.Success.Items.Count; i++)
            {
                var instance = itemsCallback.Success.Items[i].Type.GetInstance(itemsCallback.Success.Items[i]);

                if (instance.VerifyChanges())
                {
                    try
                    {
                        if (_agentService.Send(instance).IsFailure)
                            GenerateFile(instance);
                    }
                    catch (Exception ex)
                    {
                        GenerateFile(instance);
                        GenerateLogException(ex, instance);
                    }
                }
                itemsCallback.Success.Items[i] = instance;
            }
        }

        private void ReceivedMessage(object obj)
        {
            try
            {
                var solicitation = JsonConvert.DeserializeObject<ItemSolicitationVO>(obj.ToString());

                if (solicitation != null)
                {
                    var item = _items.Success.Items.FirstOrDefault(x => x.Id == solicitation.ItemId);

                    if (item != null)
                    {
                        var instance = item.Type.GetInstance(item);

                        try
                        {
                            instance.Change(solicitation.NewValue, solicitation.SolicitationType);

                            if (_agentService.Send(instance).IsFailure)
                                GenerateFile(instance);
                        }
                        catch (Exception ex)
                        {
                            GenerateLogException(ex, instance);
                        }
                    }
                    else
                    {
                        var msg = $"Não foi encontrado nenhum item com nome: {solicitation.Name}\n";
                        msg += $"ou com DisplayName: {solicitation.DisplayName}\n";
                        GenerateLogException(new Exception(msg));
                    }
                }
            }
            catch (Exception ex)
            {
                GenerateLogException(ex);
            }
        }
    }
}
