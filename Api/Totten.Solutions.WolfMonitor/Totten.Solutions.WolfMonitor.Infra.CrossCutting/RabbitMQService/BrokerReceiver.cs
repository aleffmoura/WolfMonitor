using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Interfaces;

namespace Totten.Solutions.WolfMonitor.Infra.CrossCutting.RabbitMQService
{
    public class BrokerReceiver : BackgroundService
    {
        private readonly IRabbitMQ _rabbitMQ;
        private readonly IConfigurationRoot _configuration;
        private readonly ILogger<BrokerReceiver> _logger;
        private readonly IHelper _helper;

        public static event Action<object> MessageReceived;


        public BrokerReceiver(IRabbitMQ rabbitMQ,
                              IConfigurationRoot configuration,
                              ILogger<BrokerReceiver> logger,
                              IHelper helper)
        {
            this._rabbitMQ = rabbitMQ;
            this._configuration = configuration;
            this._logger = logger;
            this._helper = helper;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Factory.StartNew(() =>
            {
                try
                {
                    _rabbitMQ.Receive((msg) =>
                    {
                        Console.WriteLine($"Message received on service {_helper.GetServiceName()} at {DateTime.Now.ToString()}!");
                        if (MessageReceived != null)
                            MessageReceived.Invoke(msg);
                    }, stoppingToken);
                }
                catch (Exception exc)
                {
                    _logger.LogError($"Failed to connect to RabbitMQ: {exc.Message}", exc);
                    throw exc;
                }
            }, stoppingToken);
        }
    }
}
