using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Foundation.ImageCompression;

namespace Sitecore.Foundation.ImageCompression.Settings
{
    public static class ImageCompressionSettings
    {
        public static string GetApiEndpoint()
        {
            return GetGlobalSettingsItem().Fields[ImageCompressionConstants.GlobalSettings.Fields.EndpointUrl].Value;
        }

        public static string GetApiEndpointKey()
        {
            return GetGlobalSettingsItem().Fields[ImageCompressionConstants.GlobalSettings.Fields.EndpointKey].Value;
        }

        public static bool IsImageCompressionEnabled()
        {
            var item = GetGlobalSettingsItem();
            return item.Fields[ImageCompressionConstants.GlobalSettings.Fields.ImageCompressionEnabled].HasValue && item.Fields[ImageCompressionConstants.GlobalSettings.Fields.ImageCompressionEnabled].Value == "1";
        }

        public static bool IsImageCompressionButtonEnabled()
        {
            if (!IsImageCompressionEnabled()) return false;
            var item = GetGlobalSettingsItem();
            return item.Fields[ImageCompressionConstants.GlobalSettings.Fields.ImageCompressionButtonEnabled].HasValue && item.Fields[ImageCompressionConstants.GlobalSettings.Fields.ImageCompressionButtonEnabled].Value == "1";
        }

        public static bool IsImageCompressionScheduledTaskEnabled()
        {
            if (!IsImageCompressionEnabled()) return false;
            var item = GetGlobalSettingsItem();
            return item.Fields[ImageCompressionConstants.GlobalSettings.Fields.ImageCompressionScheduledTaskEnabled].HasValue && item.Fields[ImageCompressionConstants.GlobalSettings.Fields.ImageCompressionScheduledTaskEnabled].Value == "1";
        }

        private static Item GetGlobalSettingsItem()
        {
            return GetMasterDatabase().GetItem(ImageCompressionConstants.GlobalSettings.ImageCompressionGlobalSettingsId);
        }

        private static Database GetMasterDatabase()
        {
            return Database.GetDatabase(ImageCompressionConstants.GlobalSettings.Database.Master);
        }

    }
}