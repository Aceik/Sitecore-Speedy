using System.Web;
using RestSharp;
using RestSharp.Authenticators;
using Sitecore.Foundation.Speedy.Model;
using Sitecore.Foundation.Speedy.Settings;

namespace Sitecore.Foundation.Speedy.Services
{
    public class CriticalGenerationGateway : ICriticalGenerationGateway
    {
        public string GenerateCritical(string url, string width = "1800", string height = "1200")
        {
            var client = new RestClient(SpeedyGenerationSettings.GetCriticalApiEndpoint());
             client.Authenticator = new HttpBasicAuthenticator(SpeedyGenerationSettings.GetCriticalApiEndpointUsername(), SpeedyGenerationSettings.GetCriticalApiEndpointPassword());

            var request = new RestRequest(Method.GET);
            request.AddParameter("url", HttpUtility.UrlEncode(url)); // adds to POST or URL querystring based on Method
            request.AddParameter("width", width);
            request.AddParameter("height", height);
            
            // or automatically deserialize result
            // return content type is sniffed but can be explicitly set via RestClient.AddHandler();
            var response2 = client.Execute<CriticalJson>(request);
            return response2.Data.Result;
        }
    }
}