using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using NpgsqlTypes;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.PostgreSQL;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Interfaces;

namespace Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Logging
{
    public static class AppBuilder
    {
        internal static LoggingLevelSwitch loggingLevel = new LoggingLevelSwitch();

        public static void UseLogging(this IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            var configuration = (IConfigurationRoot)app.ApplicationServices.GetService(typeof(IConfigurationRoot));

            loggingLevel = new LoggingLevelSwitch(GetLogEventLevel(configuration["loggingLevel"]));
            TableCreator.DefaultBitColumnsLength = 20;
            TableCreator.DefaultCharColumnsLength = 50;
            TableCreator.DefaultVarcharColumnsLength = 1000;

            IDictionary<string, ColumnWriterBase> columnWriters = new Dictionary<string, ColumnWriterBase>
            {
                {"message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
                {"message_template", new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
                {"level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
                {"raise_date", new TimestampColumnWriter(NpgsqlDbType.Timestamp) },
                {"exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
                {"properties", new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) },
                {"props_test", new PropertiesColumnWriter(NpgsqlDbType.Jsonb) },
                {"machine_name", new SinglePropertyColumnWriter("MachineName", PropertyWriteMethod.ToString, NpgsqlDbType.Text, "l") }
            };

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .MinimumLevel.ControlledBy(loggingLevel)
                .WriteTo.PostgreSQL(columnOptions: columnWriters, connectionString: configuration["databaseLogging"], tableName: "logs", needAutoCreateTable: true)
                .CreateLogger();

            loggerFactory.AddSerilog();

            var helpers = (IHelper)app.ApplicationServices.GetService(typeof(IHelper));
            ChangeToken.OnChange(
                configuration.GetReloadToken,
                () =>
                {
                    loggingLevel.MinimumLevel = GetLogEventLevel(configuration["loggingLevel"]);
                    Log.Logger.Warning($"Configurations reloaded on service {helpers.GetServiceName()}");
                }
            );
        }

        internal static LogEventLevel GetLogEventLevel(string confLogLevel)
        {
            switch (confLogLevel)
            {
                case "Verbose":
                    return LogEventLevel.Verbose;

                case "Information":
                    return LogEventLevel.Information;

                case "Fatal":
                    return LogEventLevel.Fatal;

                case "Error":
                    return LogEventLevel.Error;

                case "Debug":
                    return LogEventLevel.Debug;

                default:
                    return LogEventLevel.Warning;
            }
        }

        public static void LogErrorResponses(this IApplicationBuilder app)
        {
            var helpers = (IHelper)app.ApplicationServices.GetService(typeof(IHelper));

            if (!helpers.GetServiceName().Contains("Gateway"))
                return;

            app.Use(async (context, next) =>
            {
                string ret = "";

                Stream existingBody = context.Response.Body;

                using (MemoryStream ms = new MemoryStream())
                {
                    context.Response.Body = ms;
                    await next();
                    int codHttp = context.Response.StatusCode;
                    ms.Seek(0, SeekOrigin.Begin);
                    ret = new StreamReader(ms).ReadToEnd();
                    context.Response.Body = existingBody;

                    if (!context.Response.StatusCode.ToString().StartsWith("2"))
                    {

                        ret = JsonConvert.SerializeObject(new
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = context.Response.StatusCode.ToString().StartsWith("5") ? "The server encountered an error processing the request. See server logs for more details." : ret
                        });

                        string bodyReq = new StreamReader(context.Request.Body).ReadToEnd();
                        Log.Logger.Warning(JsonConvert.SerializeObject(
                            new { Resultado = ret, context.Request.Method, context.Request.QueryString, BodyRequest = bodyReq })
                        );
                    }

                    context.Response.ContentLength = Encoding.UTF8.GetBytes(ret).Length;
                }

                await context.Response.WriteAsync(ret);
            });
        }
    }
}
