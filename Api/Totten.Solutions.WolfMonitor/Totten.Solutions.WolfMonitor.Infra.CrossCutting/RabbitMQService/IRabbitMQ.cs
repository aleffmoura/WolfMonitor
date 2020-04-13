using System;
using System.Threading;

namespace Totten.Solutions.WolfMonitor.Infra.CrossCutting.RabbitMQService
{
    public interface IRabbitMQ
    {
        void RouteMessage<TMessage>(string channel, TMessage message);
        void Broadcast<TMessage>(TMessage message);
        void Receive(Action<object> received, CancellationToken token, string queue = "");
    }
}
