using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SimpleInjector;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Auth;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Consul;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Filters;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Injector;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Logging;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Mapper;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Mediat;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Metrics;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Swagger;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Validators;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Helpers;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Interfaces;

namespace Totten.Solutions.WolfMonitor.Cfg.Startup
{
    public static class ServiceExtension
    {
        private static readonly IHelper _helper = new Helper(null);

        public static void DefaultServiceSetup(this IServiceCollection services, Container container)
        {
            services.AddConsulServiceConfigurations(new string[] { "Global", _helper.GetServiceName() });
            IConfigurationRoot configuration = services.BuildServiceProvider().GetService<IConfigurationRoot>();
            services.AddSimpleInjector(container);
            services.AddDependencies(container, configuration);
            services.AddAutoMapper(typeof(Application.Module));
            services.AddOData();
            AddCors(services, configuration);
            services.AddSingleton<IHelper, Helper>();
            services.AddSwagger();
            services.AddMediator(container);
            services.AddValidators(container);
            services.AddFilters();
            services.AddLog();
            services.AddMetric();
            //services.AddRabbitMQ();
            services.AddMvc().AddMetrics().AddJsonOptions(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddJsonOptions(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
            });
            services.AddAuth(configuration);
            services.BuildServiceProvider();
        }


        private static void AddCors(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder =>
                        builder.WithOrigins(configuration["CORS"].Split(","))
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                );
            });
        }

    }
}
