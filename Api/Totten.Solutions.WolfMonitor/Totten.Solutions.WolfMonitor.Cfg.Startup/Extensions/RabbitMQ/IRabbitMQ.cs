using System;
using System.Threading;

namespace Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.RabbitMQ
{
    public interface IRabbitMQ
    {
        void RouteMessage<TMessage>(string route, TMessage message);
        void Broadcast<TMessage>(TMessage message);
        void Receive(Action<object> received, CancellationToken token);
    }
}
