using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Totten.Solutions.WolfMonitor.Cfg.Startup.Filters
{
    public class ODataQueryOptionsValidateAttribute : ActionFilterAttribute
    {
        private ODataValidationSettings oDataValidationSettings;

        public ODataQueryOptionsValidateAttribute(AllowedQueryOptions allowedQueryOptions = AllowedQueryOptions.All ^ AllowedQueryOptions.Expand)
        {
            oDataValidationSettings = new ODataValidationSettings() { AllowedQueryOptions = allowedQueryOptions };
        }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (actionContext.ActionArguments.Any(a => a.Value != null && a.Value.GetType().Name.Contains(typeof(ODataQueryOptions).Name)))
            {
                var odataQueryOptions = (ODataQueryOptions)actionContext.ActionArguments.Where(a => a.Value != null && a.Value.GetType().Name.Contains(typeof(ODataQueryOptions).Name)).FirstOrDefault().Value;
                odataQueryOptions.Validate(oDataValidationSettings);
            }

            base.OnActionExecuting(actionContext);
        }
    }
}
