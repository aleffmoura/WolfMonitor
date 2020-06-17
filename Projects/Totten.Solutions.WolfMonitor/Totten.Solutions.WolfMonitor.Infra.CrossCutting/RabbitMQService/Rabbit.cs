using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Interfaces;

namespace Totten.Solutions.WolfMonitor.Infra.CrossCutting.RabbitMQService
{
    public class Rabbit : IRabbitMQ
    {
        private readonly IConfiguration configuration;
        private readonly IHelper _helper;
        private readonly string _exchangeName, _hostname;

        public Rabbit(IConfigurationRoot configuration, IHelper helper)
        {
            this.configuration = configuration?.GetSection("broker");
            this._helper = helper;
            this._exchangeName = configuration != null ? this.configuration["exchangeName"] : "tottem";
            this._hostname = configuration != null ? this.configuration["hostname"] : "192.168.0.102";
        }

        /// <summary>
        /// Metodo responsável por criar/registar uma exchange no rabbitMQ
        /// </summary>
        /// <param name="channel">Modelo de criação</param>
        /// <param name="queue">Nome da fila</param>
        private void SetupExchangeQueue(IModel channel, string queue = "")
        {
            channel.ExchangeDeclare(exchange: _exchangeName,
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
            var factory = new ConnectionFactory() { HostName = _hostname };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                SetupExchangeQueue(channel);

                channel.BasicPublish(exchange: _exchangeName,
                                     routingKey: "#",
                                     basicProperties: null,
                                     body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));
            }
        }
        /// <summary>
        /// Mensagem direcionada para rabbitmq
        /// </summary>
        /// <typeparam name="TMessage">Objeto T para mensagem</typeparam>
        /// <param name="queue">Rota para mensagem</param>
        /// <param name="message">Mensagem a ser enviada</param>
        public void RouteMessage<TMessage>(string queue, TMessage message)
        {
            var factory = new ConnectionFactory() { HostName = _hostname };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                SetupExchangeQueue(channel, queue);

                channel.BasicPublish(exchange: _exchangeName,
                                     routingKey: queue,
                                     body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));

            }
        }
        /// <summary>
        /// Método responsável por escutar fila do RabbitMQ
        /// </summary>
        /// <param name="received">Função para ser executada com o retorno</param>
        /// <param name="token"></param>
        public void Receive(Action<object> received, CancellationToken token, string queue = "")
        {
            var factory = new ConnectionFactory() { HostName = _hostname };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                SetupExchangeQueue(channel, queue);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (e, ea) =>
                {
                    received.Invoke(JsonConvert.DeserializeObject(Encoding.UTF8.GetString(ea.Body)));
                };

                channel.QueueBind(queue: string.IsNullOrEmpty(queue) ? _helper.GetServiceName() : queue,
                                  exchange: _exchangeName,
                                  routingKey: string.IsNullOrEmpty(queue) ? _helper.GetServiceName() : queue);


                channel.BasicConsume(queue: string.IsNullOrEmpty(queue) ? _helper.GetServiceName() : queue,
                                 autoAck: true,
                                 consumer: consumer);


                WaitHandle.WaitAny(new[] { token.WaitHandle });
            }
        }
    }
}
