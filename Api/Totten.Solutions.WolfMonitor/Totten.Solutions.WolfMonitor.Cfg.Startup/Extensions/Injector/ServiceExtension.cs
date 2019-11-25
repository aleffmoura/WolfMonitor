using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;
using System.Net.Http;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Domain.Features.Companies;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.ORM.Contexts;
using Totten.Solutions.WolfMonitor.Infra.ORM.Features.Agents;
using Totten.Solutions.WolfMonitor.Infra.ORM.Features.Companies;
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


            RegisterFeatures(container);

            services.AddScoped(s => s.GetRequiredService<IHttpClientFactory>().CreateClient());
        }

        private static void RegisterFeatures(Container container)
        {
            container.Register<IAgentRepository, AgentRepository>();
            container.Register<ICompanyRepository, CompanyRepository>();
            container.Register<IUserRepository, UserRepository>();
            container.Register<IRoleRepository, RoleRepository>();
        }
        
    }
}
