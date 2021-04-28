using Sitecore.Foundation.Speedy.Extensions;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Sitecore.Foundation.Speedy.Settings
{
    public static class SpeedyGenerationSettings
    {
        public static bool IsPublicFacingEnvironment()
        {
            return bool.Parse(Configuration.Settings.GetSetting("Speedy.IsPublicFacingEnvironment"));
        }
        public static bool UseLocalCriticalCssGenerator()
        {
            return bool.Parse(Configuration.Settings.GetSetting("Speedy.UseLocalCriticalCssGenerator"));
        }
        public static bool IsSpeedyEnabledForPage(this Item item)
        {
            return item.Fields[SpeedyConstants.Fields.SpeedyEnabled] != null && item.IsEnabled(SpeedyConstants.Fields.SpeedyEnabled) && !Sitecore.Context.PageMode.IsExperienceEditor;
        }

        public static bool IsOnePassCookieEnabled(this Item item)
        {
            return item.IsEnabled(SpeedyConstants.Fields.OnePassCookieEnabled);
        }

        public static bool IsCriticalStylesEnabled(this Item item)
        {
            return item.IsEnabled(SpeedyConstants.Fields.EnableStylesheetLoadDefer);
        }

        public static bool IsCriticalJavascriptEnabledAndPossible(this Item item)
        {
            return item.Fields[SpeedyConstants.Fields.EnableJavascriptLoadDefer] != null && item.IsEnabled(SpeedyConstants.Fields.EnableJavascriptLoadDefer);
        }

        public static Item GetGlobalSettingsItemFromContext()
        {
            return GetContextDatabase().GetItem(SpeedyConstants.GlobalSettings.SpeedyGlobalSettingsId);
        }

        private static Database GetContextDatabase()
        {
            return Sitecore.Context.Database;
        }

        public static string GetCookieExpiration()
        {
            return GetGlobalSettingsItemFromContext().Fields[SpeedyConstants.GlobalSettings.Fields.CookieExpiration].Value;
        }

        public static bool IsDebugModeEnabled()
        {
            return GetGlobalSettingsItemFromContext().IsEnabled(SpeedyConstants.GlobalSettings.Fields.EnableDebugMode);
        }

        public static string GetDeferJSLoadForMilliseconds()
        {
            return GetGlobalSettingsItemFromContext().Fields[SpeedyConstants.GlobalSettings.Fields.DeferJSLoadForMilliseconds].Value;
        }

        public static string GetDeferCSSLoadForMilliseconds()
        {
            return GetGlobalSettingsItemFromContext().Fields[SpeedyConstants.GlobalSettings.Fields.DeferCSSLoadForMilliseconds].Value;
        }

        public static string GetDeferFallbackMilliseconds()
        {
            return GetGlobalSettingsItemFromContext().Fields[SpeedyConstants.GlobalSettings.Fields.DeferFallbackForMilliseconds].Value;
        }

        public static string GetFallbackExperienceSelector()
        {
            return Context.Item.Fields[SpeedyConstants.Fields.FallbackSelectorFieldName].Value;
        }
    }
}