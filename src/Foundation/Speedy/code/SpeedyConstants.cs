using Sitecore.Data;

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
            public static readonly string SpecialCaseCriticalCss = "SpecialCaseCriticalCSS";
            public static readonly string EnableJavascriptLoadDefer = "EnableJavascriptLoadDefer";
            public static readonly string EnableStylesheetLoadDefer = "EnableStylesheetLoadDefer";
            public static readonly string OnePassCookieEnabled = "OnePassCookieEnabled";
            public static readonly string FallbackSelectorFieldName = "CssFallbackSelector";
            public static readonly string MobileCriticalJavascript = "MobileCriticalJavascript";
            public static readonly string EveryLoadVanillaJavscriptFile = "EveryLoadVanillaJavscriptFile";
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
                public static readonly string CookieExpiration = "CookieExpirationDays";
                public static readonly string DeferJSLoadForMilliseconds = "DeferJSLoadForMilliseconds";
                public static readonly string DeferCSSLoadForMilliseconds = "DeferCSSLoadForMilliseconds";
                public static readonly string DeferFallbackForMilliseconds = "DeferFallbackForMilliseconds";
                public static readonly string EnableDebugMode = "EnableDebugMode";
                
            }

            public struct Database
            {
                public static readonly string Master = "master";
            }

            public struct Index
            {
                public static readonly string Master = "sitecore_master_index";
            }
        }

        public struct ByPass
        {
            public static readonly string ByPassParameter = "speedyByPass";
        }
    }
}