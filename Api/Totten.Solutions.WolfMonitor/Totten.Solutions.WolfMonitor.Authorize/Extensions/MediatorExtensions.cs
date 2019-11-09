using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Totten.Solutions.WolfMonitor.Application;

namespace Totten.Solutions.WolfMonitor.Authorize.Extensions
{
    public static class MediatorExtensions
    {
        public static void AddMediator(this IServiceCollection services)
        {
            var assembly = typeof(Application.Module).GetTypeInfo().Assembly;

            services.AddMediatR(assembly);
        }
    }
}
