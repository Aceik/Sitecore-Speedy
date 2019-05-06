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
            AsyncHelpers.RunSync(() => Start());
            //Start().Wait();
            return "";
        }

        public static async Task Start()
        {
            try
            {
                Sitecore.Diagnostics.Log.Info("create node js", "");
                var func = Edge.Func(@"
            return function (data, callback) {
                callback(null, 'Node.js welcomes ' + data);
            }");

                Sitecore.Diagnostics.Log.Info("about to call node js", "");
                var result = (await func(".NET"));
                Sitecore.Diagnostics.Log.Info("resulted", "");
                Sitecore.Diagnostics.Log.Info(result as string, "");
                Sitecore.Diagnostics.Log.Info("finished to call node js", "");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetBaseException().Message);
                Sitecore.Diagnostics.Log.Info(ex.GetBaseException().Message, "");
            }
            
        }
    }
}