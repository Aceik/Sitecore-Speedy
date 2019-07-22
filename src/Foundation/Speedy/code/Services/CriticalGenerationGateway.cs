//  References: https://github.com/TomTyack/CriticalCSSAPI
//  Postman Examples: https://www.getpostman.com/collections/f2df62c43496395dbdd8


using System;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using Sitecore.Foundation.Speedy.Model;
using Sitecore.Foundation.Speedy.Settings;

namespace Sitecore.Foundation.Speedy.Services
{
    /// <summary>
    /// Please note that you can easily host an API that will support the call below.
    /// Its a single node javascript file.
    /// An example is available in the following Repository: https://github.com/TomTyack/CriticalCSSAPI
    /// It may be necessary to by a bundle of browser.io calls in order to support Chromium.  A free allowance is currently being provided. 
    /// </summary>
    public class CriticalGenerationGateway : ICriticalGenerationGateway
    {
        /// <summary>
        /// Calls the remote endpoint which will generate t he Critical CSS for a particular URL.
        /// </summary>
        /// <param name="url">The URL to generate the Critical CSS for.</param>
        /// <param name="width">The width of the viewport that is considered critical.</param>
        /// <param name="height">The height of the viewport that is considered critical.</param>
        /// <param name="fontReplace">If set to true the remote API will receive a POST request. The font map JSON string should be supplied in the BODY.</param>
        /// <param name="fontReplaceStr">This string represents a JSON object "fontmap". Which is an array of find and replace strings.</param>
        /// <returns></returns>
        public string GenerateCritical(string url, string width = "1800", string height = "1200", bool fontReplace = false)
        {
            url = "https://www.southaustralia.com/";
            //var endPoint = SpeedyGenerationSettings.GetCriticalApiEndpoint();
            //var endPointA = "https://nodeapicritical.azurewebsites.net/criticalfonts/?url=https%3a%2f%2fwww.southaustralia.com%2f&width=1800&height=1200";
            //var client = new RestClient(endPointA);
            ////client.Authenticator = new HttpBasicAuthenticator(SpeedyGenerationSettings.GetCriticalApiEndpointUsername(), SpeedyGenerationSettings.GetCriticalApiEndpointPassword());

            //var request = new RestRequest(Method.POST);

            ////request.AddParameter("url", HttpUtility.UrlEncode(url)); // adds to POST or URL querystring based on Method
            ////request.AddParameter("width", width);
            ////request.AddParameter("height", height);

            ////var client = new RestClient("https://nodeapicritical.azurewebsites.net/criticalfonts/?url=https%3a%2f%2fwww.southaustralia.com%2f&width=1800&height=1200");

            //request.AddHeader("cache-control", "no-cache");
            //request.AddHeader("content-length", "18");
            //request.AddHeader("accept-encoding", "gzip, deflate");
            //request.AddHeader("Host", "nodeapicritical.azurewebsites.net");
            //request.AddHeader("Cache-Control", "no-cache");
            //request.AddHeader("Accept", "*/*");
            //request.AddHeader("User-Agent", "PostmanRuntime/7.15.0");
            //request.AddHeader("Authorization", "Basic Y3JpdGljYWw6Z2VuZXJhdG9y");
            //request.AddHeader("Content-Type", "application/json");
            //request.AddParameter("application/json", "{ \"fontmap\": []  }", ParameterType.RequestBody);
            //IRestResponse response = client.Execute(request);
            //try
            //{
            //    var response2 = client.Execute<CriticalJson>(request);
            //    var content = response2.Content;
            //    return response2.Data.Result;
            //}
            //catch (Exception ex)
            //{
            //    Diagnostics.Log.Error("Speedy Remote API caused an error", ex);
            //}

            url = "https://www.southaustralia.com/";
            var client = new RestClient(SpeedyGenerationSettings.GetCriticalApiEndpoint());
            //client.Authenticator = new HttpBasicAuthenticator(SpeedyGenerationSettings.GetCriticalApiEndpointUsername(), SpeedyGenerationSettings.GetCriticalApiEndpointPassword());

            var request = new RestRequest(Method.POST);

            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-length", "18");
            request.AddHeader("accept-encoding", "gzip, deflate");
            request.AddHeader("Host", "nodeapicritical.azurewebsites.net");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent", "PostmanRuntime/7.15.0");
            request.AddHeader("Authorization", "Basic Y3JpdGljYWw6Z2VuZXJhdG9y");
            request.AddHeader("Content-Type", "application/json");

            request.RequestFormat = DataFormat.Json;
            var fontReplaceStr = SpeedyGenerationSettings.GetCriticalApiRemoteFontMap();
            if (fontReplace && !string.IsNullOrWhiteSpace(fontReplaceStr))
            {
                fontReplaceStr = fontReplaceStr.Replace("\r", string.Empty);
                fontReplaceStr = fontReplaceStr.Replace("\n", string.Empty);
                var requestBody = new RemoteCriticalParameters();

                requestBody.FontMap = JsonConvert.DeserializeObject<RemoteCriticalParameters>(fontReplaceStr).FontMap;
                requestBody.Height = Int32.Parse(height);
                requestBody.Width = Int32.Parse(width);
                requestBody.Url = HttpUtility.UrlEncode(url);
                request.AddParameter("application/json", JsonConvert.SerializeObject(requestBody), ParameterType.RequestBody);
                //request.AddBody(requestBody);
            }

            client.Timeout = 300000;

            // or automatically deserialize result
            // return content type is sniffed but can be explicitly set via RestClient.AddHandler();
            try
            {
                var response2 = client.Execute<CriticalJson>(request);
                var content = response2.Content;
                return response2.Data.Result;
            }
            catch (Exception ex)
            {
                Diagnostics.Log.Error("Speedy Remote API caused an error", ex);
            }
            return "API ISSUE";
        }

        /**
         * Font Map example:
         *
         
         { "fontmap": [
	        {
              "find": "../../",
              "replace": "BALLLLZZZZZZZ"
            },{
              "find": "/C:/Users/thomas/AppData/Local/fonts/",
              "replace": ""
            },
            {
              "find": "fontawesome-webfont-woff.woff?v=4.7.0",
              "replace": "/-/media/Base-Themes/Core-Libraries/fonts/fontawesome/fontawesome-webfont-woff.woff?v=4.7.0"
            }
        ]}


         */
    }
}