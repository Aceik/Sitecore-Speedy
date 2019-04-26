using RestSharp;
using Site.Foundation.PageSpeed.Model;
using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.Foundation.PageSpeed.Extensions
{
    public static class QuickExtensions
    {
        public static bool IsEnabled(this Item item, string fieldName)
        {
            return item.Fields[fieldName].HasValue && item.Fields[fieldName].Value == "1";
        }
    }
}