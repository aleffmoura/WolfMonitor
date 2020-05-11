using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.OData.UriParser;

namespace Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Filters
{
    public static class AppBuilder
    {
        public static void UseOData(this IApplicationBuilder app)
        {
            // Habilitando o ODATA
            app.UseMvc(routebuilder =>
            {
                routebuilder.EnableDependencyInjection(builder =>
                {
                    builder.AddService(Microsoft.OData.ServiceLifetime.Singleton, typeof(ODataUriResolver), sp => new StringAsEnumResolver() { EnableCaseInsensitive = true });
                });
                routebuilder
                    .Select()
                    .Filter()
                    .OrderBy()
                    .MaxTop(int.MaxValue)
                    .Count();
            });
        }
    }
}
