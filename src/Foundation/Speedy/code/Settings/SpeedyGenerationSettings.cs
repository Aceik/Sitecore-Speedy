using Sitecore.Foundation.Speedy.Extensions;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Sitecore.Foundation.Speedy.Settings
{
    public class SpeedyGenerationSettings
    {
        public static string GetCriticalApiEndpoint()
        {
            return GetGlobalSettingsItem().Fields[SpeedyConstants.GlobalSettings.Fields.EndpointUrl].Value;
        }

        public static bool ShouldRegenerateOnEachSave()
        {
            var item = GetGlobalSettingsItem();
            return item.Fields[SpeedyConstants.GlobalSettings.Fields.ShouldRegenerateOnEverySaveEvent].HasValue && item.Fields[SpeedyConstants.GlobalSettings.Fields.ShouldRegenerateOnEverySaveEvent].Value == "1";
        }

        public static bool IsPublicFacingEnvironment()
        {
            return bool.Parse(Sitecore.Configuration.Settings.GetSetting("Speedy.IsPublicFacingEnvironment"));
        }

        public static bool IsSpeedyEnabledForPage(Item item)
        {
            return item.IsEnabled(SpeedyConstants.Fields.SpeedyEnabled);
        }

        public static bool IsOnePassCookieEnabled(Item item)
        {
            return item.IsEnabled(SpeedyConstants.Fields.OnePassCookieEnabled);
        }

        public static bool IsCriticalStylesEnabledAndPossible(Item item)
        {
            return item.IsEnabled(SpeedyConstants.Fields.EnableStylesheetLoadDefer) && item.Fields[SpeedyConstants.Fields.CriticalCss].HasValue;
        }

        public static bool IsCriticalJavascriptEnabledAndPossible(Item item)
        {
            return item.IsEnabled(SpeedyConstants.Fields.EnableJavascriptLoadDefer);
        }

        private static Item GetGlobalSettingsItem()
        {
            return GetMaster().GetItem(SpeedyConstants.GlobalSettings.SpeedyGlobalSettingsId);
        }

        private static Database GetMaster()
        {
            return Sitecore.Configuration.Factory.GetDatabase("master");
        }
    }
}