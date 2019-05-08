using System.Web.Http;
using Sitecore.Pipelines;

namespace Site.Foundation.PageSpeed
{
    public class Routes
    {
        public void Process(PipelineArgs args)
        {
            //GlobalConfiguration.Configure(Configure);
            HttpConfiguration httpConfig = GlobalConfiguration.Configuration;
            httpConfig.Routes.MapHttpRoute("critical", "critical/{controller}/{action}/{id}", true, new { id = RouteParameter.Optional });
        }

        protected void Configure(HttpConfiguration configuration)
        {
            //var routes = configuration.Routes;
        }
    }
}