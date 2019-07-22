//  References: https://github.com/TomTyack/CriticalCSSAPI
//  Postman Examples: https://www.getpostman.com/collections/f2df62c43496395dbdd8


using System.Web;
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
            var client = new RestClient(SpeedyGenerationSettings.GetCriticalApiEndpoint());
             client.Authenticator = new HttpBasicAuthenticator(SpeedyGenerationSettings.GetCriticalApiEndpointUsername(), SpeedyGenerationSettings.GetCriticalApiEndpointPassword());

            var method = Method.GET;
            if (fontReplace)
                method = Method.POST;
            var request = new RestRequest(method);
            request.AddParameter("url", HttpUtility.UrlEncode(url)); // adds to POST or URL querystring based on Method
            request.AddParameter("width", width);
            request.AddParameter("height", height);

            var fontReplaceStr = SpeedyGenerationSettings.GetCriticalApiRemoteFontMap();
            if (fontReplace && !string.IsNullOrWhiteSpace(fontReplaceStr))
            {
                request.AddBody(fontReplaceStr);
            }

            // or automatically deserialize result
            // return content type is sniffed but can be explicitly set via RestClient.AddHandler();
            var response2 = client.Execute<CriticalJson>(request);
            return response2.Data.Result;
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