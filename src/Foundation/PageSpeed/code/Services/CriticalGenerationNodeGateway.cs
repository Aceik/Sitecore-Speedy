using RestSharp;
using Site.Foundation.PageSpeed.Model;
using Site.Foundation.PageSpeed.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Threading.Tasks;
using EdgeJs;
using Site.Foundation.PageSpeed.Async;

namespace Site.Foundation.PageSpeed.Repositories
{
    public class CriticalGenerationNodeGateway : ICriticalGenerationGateway
    {
        public string GenerateCritical(string url, string width = "1800", string height = "1200")
        {
            return AsyncHelpers.RunSync(() => Start(url, width, height));
        }

        public static async Task<string> Start(string url, string width = "1800", string height = "1200")
        {
            try
            {
                Sitecore.Diagnostics.Log.Info("--- Critical Node JS --- About to read in javascript file ---", "");
                var path = System.Web.HttpContext.Current.Server.MapPath("~/Scripts/Critical.js");
                var javascriptContent = System.IO.File.ReadAllText(path);
                var func = Edge.Func(javascriptContent);

                Sitecore.Diagnostics.Log.Info("--- Critical Node JS Function About to Start ---", "");
                string input = $"{url}||30000||{width}||{height}";
                var result = (await func(input)) as string;

                Sitecore.Diagnostics.Log.Info("--- Critical Node JS Function Resulted ---", "");
                Sitecore.Diagnostics.Log.Info(result as string, "");
                return result as string;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetBaseException().Message);
                Sitecore.Diagnostics.Log.Info(ex.GetBaseException().Message, "");
            }
            return "--- Critical Node JS Generation failed ---";
        }
    }
}