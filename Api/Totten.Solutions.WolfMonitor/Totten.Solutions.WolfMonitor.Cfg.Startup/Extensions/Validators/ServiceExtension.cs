using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using System.Reflection;

namespace Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Validators
{
    public static class ServiceExtension
    {
        public static void AddValidators(this IServiceCollection services, Container container)
        {
            container.Collection.Register(typeof(IValidator<>), typeof(Application.Module).GetTypeInfo().Assembly);
        }
    }
}
