using RestSharp;
using Site.Foundation.PageSpeed.Model;
using Site.Foundation.PageSpeed.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.Foundation.PageSpeed.Repositories
{
    public class CriticalGenerationGateway : ICriticalGenerationGateway
    {
        public string GenerateCritical(string url, string width = "1800", string height = "1200")
        {
            var client = new RestClient(SpeedyGenerationSettings.GetCriticalApiEndpoint());
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

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