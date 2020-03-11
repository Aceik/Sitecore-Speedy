using System;
using Newtonsoft.Json;
using RestSharp;
using Sitecore.Foundation.Speedy.Model;
using Sitecore.Foundation.Speedy.Settings;

namespace Sitecore.Foundation.Speedy.Services
{
    public class CriticalGenerationLocal : ICriticalGenerationGateway
    {
        /// <summary>
        /// Calls the remote endpoint which will generate t he Critical CSS for a particular URL.
        /// </summary>
        /// <param name="url">The URL to generate the Critical CSS for.</param>
        /// <param name="width">The width of the viewport that is considered critical.</param>
        /// <param name="height">The height of the viewport that is considered critical.</param>
        /// <param name="fontReplace">If set to true the remote API will receive a POST request. The font map JSON string should be supplied in the BODY.</param>
        /// <returns></returns>
        public string GenerateCritical(string url, string width = "1800", string height = "1200", bool fontReplace = false)
        {

            var client = new RestClient(SpeedyGenerationSettings.GetCriticalApiEndpoint());

            var request = new RestRequest(Method.POST) { RequestFormat = DataFormat.Json };


            if (fontReplace)
            {
                var fontReplaceStr = GetRemoteJsonField(SpeedyGenerationSettings.GetCriticalApiRemoteFontMap());
                var duplicatesStr = GetRemoteJsonField(SpeedyGenerationSettings.GetCriticalApiRemoteDuplicates());
                var fontSwapStr = GetRemoteJsonField(SpeedyGenerationSettings.GetCriticalApiRemoteFontSwitch());

                var requestBody = new RemoteCriticalParameters();

                if (!string.IsNullOrWhiteSpace(fontReplaceStr))
                    requestBody.FontMap = JsonConvert.DeserializeObject<RemoteCriticalParameters>(fontReplaceStr).FontMap;
                if (!string.IsNullOrWhiteSpace(duplicatesStr))
                    requestBody.RemoveDuplicates = JsonConvert.DeserializeObject<RemoteCriticalParameters>(duplicatesStr).RemoveDuplicates;
                if (!string.IsNullOrWhiteSpace(fontSwapStr))
                    requestBody.FontFaceSwitch = JsonConvert.DeserializeObject<RemoteCriticalParameters>(fontSwapStr).FontFaceSwitch;

                requestBody.Height = height;
                requestBody.Width = width;
                requestBody.Url = url;
                request.AddParameter("application/json", JsonConvert.SerializeObject(requestBody), ParameterType.RequestBody);
            }

            client.Timeout = 300000;

            // or automatically deserialize result
            // return content type is sniffed but can be explicitly set via RestClient.AddHandler();
            try
            {
                var response2 = client.Execute(request);
                return response2.Content;
            }
            catch (Exception ex)
            {
                Diagnostics.Log.Error("Speedy Remote API caused an error", ex, this);
            }
            return "API ISSUE";
        }

        public string GetRemoteJsonField(string fieldValue)
        {
            var fontReplaceStr = fieldValue;
            fontReplaceStr = fontReplaceStr.Replace("\r", string.Empty);
            fontReplaceStr = fontReplaceStr.Replace("\n", string.Empty);
            return fontReplaceStr;
        }

    }
}