using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using System.Collections.Generic;
using System.Reflection;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Behaviours;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.RabbitMQ;

namespace Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Mediat
{
    public static class ServiceExtension
    {
        public static void AddMediator(this IServiceCollection services, Container container)
        {
            var assembly = GetAssemblies();

            container.RegisterSingleton<IMediator, Mediator>();
            container.Register(() => new ServiceFactory(container.GetInstance), Lifestyle.Singleton);
            container.Register(typeof(IRequestHandler<,>), assembly);
            container.Register(typeof(IRequestHandler<>), assembly);

            var notificationHandlerTypes = container.GetTypesToRegister(typeof(INotificationHandler<>), assembly);
            container.Collection.Register(typeof(INotificationHandler<>), notificationHandlerTypes);

            container.Collection.Register(typeof(IPipelineBehavior<,>), new[]
            {
                typeof(ValidationPipeline<,>)
            });
            services.AddMediatR(typeof(Application.Module).GetTypeInfo().Assembly);
        }

        private static IEnumerable<Assembly> GetAssemblies()
        {
            yield return typeof(IMediator).GetTypeInfo().Assembly;
            yield return typeof(Application.Module).GetTypeInfo().Assembly;
        }
    }
}
