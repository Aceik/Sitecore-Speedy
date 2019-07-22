﻿using Sitecore.Data;

namespace Sitecore.Foundation.Speedy
{
    public struct SpeedyConstants
    {
        public struct TemplateIDs
        {
            public static readonly ID SpeedyPageTemplateId = new ID("{AFB9A38D-7ECA-4C13-848C-F6A5DF08C11E}");
        }

        public struct Fields
        {
            public static readonly string SpeedyEnabled = "SpeedyEnabled";
            public static readonly string CriticalViewPortWidth = "CriticalViewPortWidthPixels";
            public static readonly string CriticalViewPortHeight = "CriticalViewPortHeightPixels";
            public static readonly string CriticalCss = "CriticalCSS";
            public static readonly string SpecialCaseCriticalCss = "SpecialCaseCriticalCSS";

            public static readonly string EnableJavascriptLoadDefer = "EnableJavascriptLoadDefer";
            public static readonly string EnableStylesheetLoadDefer = "EnableStylesheetLoadDefer";
            public static readonly string OnePassCookieEnabled = "OnePassCookieEnabled";
            
            public static readonly string FallbackSelectorFieldName = "CssFallbackSelector";
        }

        public struct CookieNames
        {
            public static readonly string OnePassCookieName = "CriticalPageLoadCookie";
        }
    
        public struct GlobalSettings
        {
            public static readonly ID SpeedyGlobalSettingsId = new ID("{7C852721-7717-41CF-B729-473859228964}");

            public struct Fields
            {
                public static readonly string EndpointUrl = "EndpointURL";
                public static readonly string EndpointUsername = "EndpointUsername";
                public static readonly string EndpointPassword = "EndpointPassword";
                public static readonly string RemoteFontMap = "RemoteFontMap";
                public static readonly string ShouldRegenerateOnEverySaveEvent = "ShouldRegenerateOnEverySaveEvent";
                public static readonly string CookieExpiration = "CookieExpirationDays";
                public static readonly string DeferJSLoadForMilliseconds = "DeferJSLoadForMilliseconds";
                public static readonly string DeferCSSLoadForMilliseconds = "DeferCSSLoadForMilliseconds";
                public static readonly string DeferFallbackForMilliseconds = "DeferFallbackForMilliseconds";
            }
        }

        public struct ByPass
        {
            public static readonly string ByPassParameter = "speedyByPass";
        }
    }
}