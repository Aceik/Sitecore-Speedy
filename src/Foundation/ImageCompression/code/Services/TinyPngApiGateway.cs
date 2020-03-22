//  References: https://github.com/TomTyack/CriticalCSSAPI
//  Postman Examples: https://www.getpostman.com/collections/f2df62c43496395dbdd8


using System;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using Sitecore.Data.Items;
using Sitecore.Foundation.ImageCompression.Model;
using Sitecore.Foundation.ImageCompression.Services;
using Sitecore.Foundation.ImageCompression.Settings;

namespace Sitecore.Foundation.ImageCompression.Services
{
    /// <summary>
    /// Call 
    /// </summary>
    public class TinyPngApiGateway : IImageCompressionService
    {
        public string CompressImage(Item currentItem)
        {
            var client = new RestClient(ImageCompressionSettings.GetApiEndpoint());
            client.Authenticator = new HttpBasicAuthenticator("Api", ImageCompressionSettings.GetApiEndpointKey());

            var request = new RestRequest(Method.POST);

            request.AddHeader("accept-encoding", "gzip, deflate");
            request.AddHeader("Host", "nodeapicritical.azurewebsites.net");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("Content-Type", "application/json");

            //ImageUpload img = new ImageUpload()
            //{
            //    InputObj = new ImageUpload.Input()
            //    {
            //        Size = 100,
            //        Type = "image/jpeg"
            //    }
            //};

            request.RequestFormat = DataFormat.Json;

            client.Timeout = 300000;

            // or automatically deserialize result
            // return content type is sniffed but can be explicitly set via RestClient.AddHandler();
            try
            {
                var response2 = client.Execute<ImageUpload>(request);
                var content = response2.Content;
                return response2.Data.Result;
            }
            catch (Exception ex)
            {
                Diagnostics.Log.Error("Speedy Remote API caused an error", ex);
            }
            return "API ISSUE";
        }
    }
}