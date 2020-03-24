using Sitecore.Data;

namespace Sitecore.Foundation.ImageCompression
{
    public struct ImageCompressionConstants
    {
        public struct Messages
        {
            public static readonly string OPTIMISED_BY = "Optimised by TinyPng";
        }

        public struct TemplateIDs
        {
            public static readonly ID ImageCompressionPageTemplateId = new ID("{AFB9A38D-7ECA-4C13-848C-F6A5DF08C11E}");
            public static readonly ID ImageTemplateId = new ID("{F1828A2C-7E5D-4BBD-98CA-320474871548}");
        }

        public struct ImageFields
        {
            //public static readonly string ImageCompressionEnabled = "ImageCompressionEnabled";
        }
    
        public struct GlobalSettings
        {
            public static readonly ID ImageCompressionGlobalSettingsId = new ID("{E25A860E-B060-4AAB-9E6B-91546EFB08CC}");

            public struct Fields
            {
                public static readonly string ImageCompressionEnabled = "ImageCompressionEnabled";
                public static readonly string ImageCompressionButtonEnabled = "ImageCompressionButtonEnabled";
                public static readonly string ImageCompressionScheduledTaskEnabled = "ImageCompressionScheduledTaskEnabled";
                public static readonly string EndpointUrl = "EndpointURL";
                public static readonly string EndpointKey = "EndpointKey";
                public static readonly string ImageCompressionInformationField = "ImageCompressionInformationField";


                
                //public static readonly string EndpointUsername = "EndpointUsername";
                //public static readonly string EndpointPassword = "EndpointPassword";
                //public static readonly string RemoteFontMap = "RemoteFontMap";
                //public static readonly string RemoteDuplicatesToRemove = "RemoteDuplicatesToRemove";
                //public static readonly string RemoteFontsToSwitch = "RemoteFontsToSwitch";
                //public static readonly string ShouldRegenerateOnEverySaveEvent = "ShouldRegenerateOnEverySaveEvent";
                //public static readonly string ShouldGenerateOnScheduledTask = "ShouldGenerateOnScheduledTask";
                //public static readonly string CookieExpiration = "CookieExpirationDays";
                //public static readonly string DeferJSLoadForMilliseconds = "DeferJSLoadForMilliseconds";
                //public static readonly string DeferCSSLoadForMilliseconds = "DeferCSSLoadForMilliseconds";
                //public static readonly string DeferFallbackForMilliseconds = "DeferFallbackForMilliseconds";

                //public static readonly string DefaultCriticalWidth = "DefaultCriticalWidth";
                //public static readonly string DefaultCriticalHeight = "DefaultCriticalHeight";
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
    }
}