using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Text;

namespace Totten.Solutions.WolfMonitor.Infra.CrossCutting.Telemetry
{
    public static class TelemetryLog
    {
        private static TelemetryClient _telemetryClient = new TelemetryClient();

        public static void SendTelemetry(string message, DateTimeOffset startTime, TimeSpan duration, bool success)
        {
            _telemetryClient.TrackDependency("MongoDB", "MongoDB", message, startTime, duration, success);
        }
    }
}
