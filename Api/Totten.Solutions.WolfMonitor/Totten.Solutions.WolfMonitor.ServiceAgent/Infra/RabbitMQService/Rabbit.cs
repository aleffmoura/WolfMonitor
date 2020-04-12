using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Infra.RabbitMQService
{
    public class Rabbit : IRabbitMQ
    {
        private readonly string exchangeName, hostname;
        private readonly string queue;
        public Rabbit(string queue)
        {
            this.exchangeName = "";
            this.hostname = "";
            this.queue = queue;
        }

        /// <summary>
        /// Metodo responsável por criar/registar uma exchange no rabbitMQ
        /// </summary>
        /// <param name="channel">Modelo de criação</param>
        /// <param name="queue">Nome da fila</param>
        private void SetupExchangeQueue(IModel channel, string queue)
        {
            channel.ExchangeDeclare(exchange: exchangeName,
                        type: "topic");

            channel.QueueDeclare(queue:  queue,
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
                SetupExchangeQueue(channel, this.queue);

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
                SetupExchangeQueue(channel, this.queue);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (e, ea) =>
                {
                    received.Invoke(JsonConvert.DeserializeObject(Encoding.UTF8.GetString(ea.Body)));
                };

                channel.QueueBind(queue: this.queue,
                                  exchange: exchangeName,
                                  routingKey: this.queue);


                channel.BasicConsume(queue: this.queue,
                                 autoAck: true,
                                 consumer: consumer);


                WaitHandle.WaitAny(new[] { token.WaitHandle });
            }
        }
    }
}
