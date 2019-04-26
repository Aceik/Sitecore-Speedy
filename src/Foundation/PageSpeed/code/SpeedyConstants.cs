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
            public static readonly string CriticalViewPortWidth = "CriticalViewPortWidthPixels";
            public static readonly string CriticalViewPortHeight = "CriticalViewPortHeightPixels";
            public static readonly string CriticalCSS = "CriticalCSS";
            
            public static readonly string EnableJavascriptLoadDefer = "EnableJavascriptLoadDefer";
            public static readonly string EnableStylesheetLoadDefer = "EnableStylesheetLoadDefer";
            public static readonly string OnePassCookieEnabled = "OnePassCookieEnabled";
        }

        public struct GlobalSettings
        {
            public static readonly ID SpeedyGlobalSettingsID = new ID("{7C852721-7717-41CF-B729-473859228964}");

            public struct Fields
            {
                public static readonly string EndpointURL = "EndpointURL";
                public static readonly string ShouldRegenerateOnEverySaveEvent = "ShouldRegenerateOnEverySaveEvent";
            }
        }

        public struct ByPass
        {
            public static readonly string ByPassParameter = "speedyByPass";
        }
    }
}