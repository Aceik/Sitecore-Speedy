using RestSharp;
using Site.Foundation.PageSpeed.Extensions;
using Site.Foundation.PageSpeed.Model;
using Site.Foundation.PageSpeed.Repositories;
using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.Foundation.PageSpeed.Settings
{
    public class SpeedyGenerationSettings
    {
        public static string GetCriticalApiEndpoint()
        {
            return GetGlobalSettingsItem().Fields[SpeedyConstants.GlobalSettings.Fields.EndpointURL].Value;
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
            return item.IsEnabled(SpeedyConstants.Fields.EnableStylesheetLoadDefer) && item.Fields[SpeedyConstants.Fields.CriticalCSS].HasValue;
        }

        public static bool IsCriticalJavascriptEnabledAndPossible(Item item)
        {
            return item.IsEnabled(SpeedyConstants.Fields.EnableJavascriptLoadDefer);
        }

        private static Item GetGlobalSettingsItem()
        {
            return GetMaster().GetItem(SpeedyConstants.GlobalSettings.SpeedyGlobalSettingsID);
        }

        private static Database GetMaster()
        {
            return Sitecore.Configuration.Factory.GetDatabase("master");
        }
    }
}