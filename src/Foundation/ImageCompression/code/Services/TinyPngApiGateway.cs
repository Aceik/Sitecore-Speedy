//  References: https://github.com/TomTyack/CriticalCSSAPI
//  Postman Examples: https://www.getpostman.com/collections/f2df62c43496395dbdd8


using System;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;
using RestSharp;
using Sitecore.Data.Items;
using Sitecore.Foundation.ImageCompression.Services;

namespace Sitecore.Foundation.ImageCompression.Services
{
    /// <summary>
    /// Call 
    /// </summary>
    public class TinyPngApiGateway : IImageCompressionService
    {
        public string CompressImage(Item currentItem)
        {
            return "";
        }
    }
}