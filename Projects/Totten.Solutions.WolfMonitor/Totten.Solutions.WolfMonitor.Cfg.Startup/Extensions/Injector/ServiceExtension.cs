using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;
using System.Net.Http;
using Totten.Solutions.WolfMonitor.Application.Features.Services;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.RabbitMQ;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents.Profiles;
using Totten.Solutions.WolfMonitor.Domain.Features.Companies;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;
using Totten.Solutions.WolfMonitor.Domain.Features.Logs;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Helpers;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Interfaces;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.RabbitMQService;
using Totten.Solutions.WolfMonitor.Infra.ORM.Contexts;
using Totten.Solutions.WolfMonitor.Infra.ORM.Features.Agents;
using Totten.Solutions.WolfMonitor.Infra.ORM.Features.Agents.Profiles;
using Totten.Solutions.WolfMonitor.Infra.ORM.Features.Companies;
using Totten.Solutions.WolfMonitor.Infra.ORM.Features.Items;
using Totten.Solutions.WolfMonitor.Infra.ORM.Features.Logs;
using Totten.Solutions.WolfMonitor.Infra.ORM.Features.Users;

namespace Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Injector
{
    public static class ServiceExtension
    {
        public static void AddSimpleInjector(this IServiceCollection services, Container container)
        {
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(container));
            services.UseSimpleInjectorAspNetRequestScoping(container);
            services.EnableSimpleInjectorCrossWiring(container);
        }

        public static void AddDependencies(this IServiceCollection services,
            Container container,
            IConfiguration configuration)
        {
            container.Register(() =>
            {
                var options = new DbContextOptionsBuilder<WolfMonitorContext>().UseSqlServer(configuration["connectionString"]).Options;
                return new WolfMonitorContext(options);
            }, Lifestyle.Scoped);
            container.Register(() =>
            {
                var options = new DbContextOptionsBuilder<WolfMonitoringContext>().UseSqlServer(configuration["monitoringConnectionString"]).Options;
                return new WolfMonitoringContext(options);
            }, Lifestyle.Scoped);
            container.Register(() =>
            {
                var options = new DbContextOptionsBuilder<AuthContext>().UseSqlServer(configuration["authConnectionString"]).Options;
                return new AuthContext(options);
            }, Lifestyle.Scoped);
            container.Register(() =>
            {
                var options = new DbContextOptionsBuilder<LogContext>().UseSqlServer(configuration["logConnectionString"]).Options;
                return new LogContext(options);
            }, Lifestyle.Scoped);


            RegisterFeatures(container, (IConfigurationRoot)configuration);

            services.AddScoped(s => s.GetRequiredService<IHttpClientFactory>().CreateClient());
        }

        private static void RegisterFeatures(Container container, IConfigurationRoot configurationRoot)
        {
            container.Register<IAgentRepository, AgentRepository>();
            container.Register<IProfileRepository, ProfileRepository>();
            container.Register<ILogRepository, LogRepository>();
            container.Register<ICompanyRepository, CompanyRepository>();
            container.Register<IItemRepository, ItemRepository>();
            container.Register<IUserRepository, UserRepository>();
            container.Register<IRoleRepository, RoleRepository>();
            container.Register<IEmailService, EmailService>();

            container.Register(() => configurationRoot, Lifestyle.Scoped);
            container.Register<IHelper>(() => new Helper(configurationRoot), Lifestyle.Scoped);
            container.Register<IRabbitMQ, Rabbit>();
            //container.Register<BrokerReceiver>();
        }

    }
}
