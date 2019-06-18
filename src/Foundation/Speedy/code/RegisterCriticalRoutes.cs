using System.Web.Http;
using System.Web.Routing;
using Sitecore.Pipelines;

namespace Sitecore.Foundation.Speedy
{
    public class RegisterCriticalRoutes
    {
        public virtual void Process(PipelineArgs args)
        {
            RegisterRoute(RouteTable.Routes);
        }

        protected virtual void RegisterRoute(RouteCollection routes)
        {
            RouteTable.Routes.MapHttpRoute("routeName",
                "speedy/{controller}/{action}/{id}" /* do not include a forward slash in front of the route */
                , defaults: new { controller = "Critical", action = "Generate" } /* controller name should not have the "Controller" suffix */
            );
        }
    }
}