using System;
using System.Threading;

namespace Totten.Solutions.WolfMonitor.Infra.CrossCutting.Totten.Solutions.WolfMonitor.ServiceAgent.Infra.RabbitMQService
{
    public interface IRabbitMQ
    {
        void RouteMessage<TMessage>(string channel, TMessage message);
        void Broadcast<TMessage>(TMessage message);
        void Receive(Action<object> received, CancellationToken token);
    }
}
