using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.Foundation.PageSpeed
{
    public struct SpeedyConstants
    {
        public struct TemplateIDs
        {
            public static readonly ID SpeedyPageTemplateID = new ID("{AFB9A38D-7ECA-4C13-848C-F6A5DF08C11E}");
        }

        public struct Fields
        {
            public static readonly string SpeedyEnabled = "SpeedyEnabled";
        }
    }
}