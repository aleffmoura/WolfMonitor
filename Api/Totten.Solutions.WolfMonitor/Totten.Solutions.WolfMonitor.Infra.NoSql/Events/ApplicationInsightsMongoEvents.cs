using MongoDB.Bson;
using MongoDB.Driver.Core.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Telemetry;

namespace Totten.Solutions.WolfMonitor.Infra.NoSql.Events
{
    public class ApplicationInsightsMongoEvents : IEventSubscriber
    {
        // Comandos do MongoDB que não precisam ser monitorados
        private readonly List<string> _notTrackedCommands = new List<string> { "ismaster", "buildinfo", "getlasterror", "saslstart", "saslcontinue", "listindexes" };

        private readonly ConcurrentDictionary<int, string> _queriesBuffer = new ConcurrentDictionary<int, string>();
        private ReflectionEventSubscriber _subscriber;

        public ApplicationInsightsMongoEvents()
        {
            _subscriber = new ReflectionEventSubscriber(this);
        }

        public bool TryGetEventHandler<TEvent>(out Action<TEvent> handler)
        {
            return _subscriber.TryGetEventHandler(out handler);
        }

        public void Handle(CommandStartedEvent started)
        {
            if (started.Command != null && !_notTrackedCommands.Contains(started.CommandName.ToLower()))
                _queriesBuffer.TryAdd(started.RequestId, started.Command.ToString());

            Debug.WriteLine($"{started.CommandName} - {started.Command.ToJson()}");
        }

        public void Handle(CommandSucceededEvent succeeded)
        {
            if (_notTrackedCommands.Contains(succeeded.CommandName.ToLower()))
                return;

            if (_queriesBuffer.TryRemove(succeeded.RequestId, out var query))
                TelemetryLog.SendTelemetry(query, DateTime.Now.Subtract(succeeded.Duration), succeeded.Duration, true);
        }

        public void Handle(CommandFailedEvent failed)
        {
            if (_notTrackedCommands.Contains(failed.CommandName.ToLower()))
                return;

            if (_queriesBuffer.TryRemove(failed.RequestId, out var query))
                TelemetryLog.SendTelemetry(query, DateTime.Now.Subtract(failed.Duration), failed.Duration, false);
        }
    }
}
