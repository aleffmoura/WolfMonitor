using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Interfaces;

namespace Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.RabbitMQ
{
    internal class Rabbit : IRabbitMQ
    {
        private readonly IConfiguration configuration;
        private readonly IHelper _helper;
        private readonly string exchangeName, hostname;

        public Rabbit(IConfigurationRoot configuration, IHelper helper)
        {
            this.configuration = configuration.GetSection("broker");
            this._helper = helper;
            this.exchangeName = this.configuration["exchangeName"];
            this.hostname = this.configuration["hostname"];
        }

        /// <summary>
        /// Metodo responsável por criar/registar uma exchange no rabbitMQ
        /// </summary>
        /// <param name="channel">Modelo de criação</param>
        /// <param name="queue">Nome da fila</param>
        private void SetupExchangeQueue(IModel channel, string queue = "")
        {
            channel.ExchangeDeclare(exchange: exchangeName,
                        type: "topic");

            channel.QueueDeclare(queue: string.IsNullOrEmpty(queue) ? _helper.GetServiceName() : queue,
                                exclusive: false,
                                durable: true);
        }
        /// <summary>
        /// Mensagem para todas as filas do rabbitmq
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="message">Mensagem a ser enviada</param>
        public void Broadcast<TMessage>(TMessage message)
        {
            var factory = new ConnectionFactory() { HostName = hostname };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                SetupExchangeQueue(channel);

                channel.BasicPublish(exchange: exchangeName,
                                     routingKey: "#",
                                     basicProperties: null,
                                     body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));
            }
        }
        /// <summary>
        /// Mensagem direcionada para rabbitmq
        /// </summary>
        /// <typeparam name="TMessage">Objeto T para mensagem</typeparam>
        /// <param name="routingKey">Rota para mensagem</param>
        /// <param name="message">Mensagem a ser enviada</param>
        public void RouteMessage<TMessage>(string routingKey, TMessage message)
        {
            var factory = new ConnectionFactory() { HostName = hostname };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                SetupExchangeQueue(channel, routingKey);

                channel.BasicPublish(exchange: exchangeName,
                                     routingKey: routingKey,
                                     body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));

            }
        }
        /// <summary>
        /// Método responsável por escutar fila do RabbitMQ
        /// </summary>
        /// <param name="received">Função para ser executada com o retorno</param>
        /// <param name="token"></param>
        public void Receive(Action<object> received, CancellationToken token)
        {
            var factory = new ConnectionFactory() { HostName = hostname };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                SetupExchangeQueue(channel);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (e, ea) =>
                {
                    received.Invoke(JsonConvert.DeserializeObject(Encoding.UTF8.GetString(ea.Body)));
                };

                channel.QueueBind(queue: _helper.GetServiceName(),
                                  exchange: exchangeName,
                                  routingKey: _helper.GetServiceName());


                channel.BasicConsume(queue: _helper.GetServiceName(),
                                 autoAck: true,
                                 consumer: consumer);


                WaitHandle.WaitAny(new[] { token.WaitHandle });
            }
        }
    }
}
