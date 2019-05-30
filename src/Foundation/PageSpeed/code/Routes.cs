using System.Web.Http;
using Sitecore.Pipelines;
using Newtonsoft.Json;
using Sitecore.Pipelines;
using Newtonsoft.Json.Serialization;
using System.Web.Routing;
using System.Web.Mvc;

namespace Site.Foundation.PageSpeed
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