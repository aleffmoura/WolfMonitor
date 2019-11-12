using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.ORM.Contexts;
using Totten.Solutions.WolfMonitor.Infra.ORM.Features.Users;


namespace Totten.Solutions.WolfMonitor.IdentityServer.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthContext>(opt => opt.UseSqlServer(configuration["connectionString"]));
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
        }
    }
}
