using Microsoft.Extensions.DependencyInjection;
using System;

namespace Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Mapper
{
    public static class ServiceExtension
    {
        public static void AddAutoMapper(this IServiceCollection services, params Type[] types)
        {
            AutoMapper.Mapper.Reset();
            AutoMapper.Mapper.Initialize(cfg =>
            {
                foreach (Type type in types)
                {
                    cfg.AddProfiles(type);
                }
            });
        }
    }
}
