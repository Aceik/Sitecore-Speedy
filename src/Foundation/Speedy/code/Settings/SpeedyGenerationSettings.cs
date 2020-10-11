using Sitecore.Foundation.Speedy.Extensions;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Sitecore.Foundation.Speedy.Settings
{
    public static class SpeedyGenerationSettings
    {
        public static string GetCriticalApiEndpoint()
        {
            return GetGlobalSettingsItem().Fields[SpeedyConstants.GlobalSettings.Fields.EndpointUrl].Value;
        }

        public static string GetCriticalApiEndpointUsername()
        {
            return GetGlobalSettingsItem().Fields[SpeedyConstants.GlobalSettings.Fields.EndpointUsername].Value;
        }

        public static string GetCriticalApiEndpointPassword()
        {
            return GetGlobalSettingsItem().Fields[SpeedyConstants.GlobalSettings.Fields.EndpointPassword].Value;
        }

        public static string GetCriticalApiRemoteFontMap()
        {
            return GetGlobalSettingsItem().Fields[SpeedyConstants.GlobalSettings.Fields.RemoteFontMap].Value;
        }

        public static string GetCriticalApiRemoteDuplicates()
        {
            return GetGlobalSettingsItem().Fields[SpeedyConstants.GlobalSettings.Fields.RemoteDuplicatesToRemove].Value;
        }

        public static string GetCriticalApiRemoteFontSwitch()
        {
            return GetGlobalSettingsItem().Fields[SpeedyConstants.GlobalSettings.Fields.RemoteFontsToSwitch].Value;
        }

        public static bool ShouldRegenerateOnEachSave()
        {
            var item = GetGlobalSettingsItem();
            return item.Fields[SpeedyConstants.GlobalSettings.Fields.ShouldRegenerateOnEverySaveEvent].HasValue && item.Fields[SpeedyConstants.GlobalSettings.Fields.ShouldRegenerateOnEverySaveEvent].Value == "1";
        }

        public static bool ShouldGenerateViaScheduledTask()
        {
            var item = GetGlobalSettingsItem();
            return item.Fields[SpeedyConstants.GlobalSettings.Fields.ShouldGenerateOnScheduledTask].HasValue && item.Fields[SpeedyConstants.GlobalSettings.Fields.ShouldGenerateOnScheduledTask].Value == "1";
        }

        public static string GetDefaultCriticalWidth()
        {
            var item = GetGlobalSettingsItem();
            return item.Fields[SpeedyConstants.GlobalSettings.Fields.DefaultCriticalWidth].Value;
        }

        public static string GetDefaultCriticalHeight()
        {
            var item = GetGlobalSettingsItem();
            return item.Fields[SpeedyConstants.GlobalSettings.Fields.DefaultCriticalHeight].Value;
        }

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
            return item.Fields[SpeedyConstants.Fields.SpeedyEnabled] != null && item.IsEnabled(SpeedyConstants.Fields.SpeedyEnabled);
        }

        public static bool IsOnePassCookieEnabled(this Item item)
        {
            return item.IsEnabled(SpeedyConstants.Fields.OnePassCookieEnabled);
        }

        public static bool IsCriticalStylesEnabledAndPossible(this Item item)
        {
            return item.IsEnabled(SpeedyConstants.Fields.EnableStylesheetLoadDefer) && item.Fields[SpeedyConstants.Fields.CriticalCss].HasValue;
        }

        public static bool IsCriticalStylesEnabled(this Item item)
        {
            return item.IsEnabled(SpeedyConstants.Fields.EnableStylesheetLoadDefer);
        }

        public static bool IsEasyCriticalEnabled(this Item item)
        {
            return item.IsCriticalStylesEnabled() && item.IsEnabled(SpeedyConstants.Fields.EnableEasyCriticalMode);
        }

        public static bool IsCriticalJavascriptEnabledAndPossible(this Item item)
        {
            return item.Fields[SpeedyConstants.Fields.EnableJavascriptLoadDefer] != null && item.IsEnabled(SpeedyConstants.Fields.EnableJavascriptLoadDefer);
        }

        private static Item GetGlobalSettingsItem()
        {
            return GetMasterDatabase().GetItem(SpeedyConstants.GlobalSettings.SpeedyGlobalSettingsId);
        }

        private static Database GetMasterDatabase()
        {
            return Database.GetDatabase(SpeedyConstants.GlobalSettings.Database.Master);
        }

        private static Item GetGlobalSettingsItemFromContext()
        {
            return GetContextDatabase().GetItem(SpeedyConstants.GlobalSettings.SpeedyGlobalSettingsId);
        }

        private static Database GetContextDatabase()
        {
            return Sitecore.Context.Database;
        }

        public static string GetCookieExpiration()
        {
            return GetGlobalSettingsItem().Fields[SpeedyConstants.GlobalSettings.Fields.CookieExpiration].Value;
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