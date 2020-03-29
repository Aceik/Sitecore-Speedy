using Sitecore.Data.Items;
using Sitecore.Foundation.Speedy.Extensions;
using Sitecore.Foundation.Speedy.Services;
using Sitecore.Foundation.Speedy.Settings;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;

namespace Sitecore.Foundation.Speedy.Repositories
{
    public class CriticalCSSRepository : ICriticalCSSRepository 
    {
        private readonly ICriticalGenerationGateway _criticalGenerationGateway;

        public CriticalCSSRepository(ICriticalGenerationGateway criticalGenerationGateway)
        {
            _criticalGenerationGateway = criticalGenerationGateway;
        }

        public void UpdateCriticalCSS(Item item, string database)
        {
            if (item == null)
                return;

            // Only act if within the master database
            if ((item.Database != null && string.CompareOrdinal(item.Database.Name, database) != 0)
                || item.Name == "__Standard Values"
                || !item.IsSpeedyEnabledForPage())
            {
                return;
            }

            if (!item.InheritsFrom(SpeedyConstants.TemplateIDs.SpeedyPageTemplateId))
                return;

            // If speedy is enabled for this page and should we generate the CSS
            if (!item.IsEnabled(SpeedyConstants.Fields.SpeedyEnabled))
                return;

            var presentUrl = item.GetUrlForContextSite() + $"?{SpeedyConstants.ByPass.ByPassParameter}=true";

            var width = item.Fields[SpeedyConstants.Fields.CriticalViewPortWidth].Value;
            var height = item.Fields[SpeedyConstants.Fields.CriticalViewPortHeight].Value;

            if (string.IsNullOrWhiteSpace(width))
                width = SpeedyGenerationSettings.GetDefaultCriticalWidth();
            if (string.IsNullOrWhiteSpace(height))
                height = SpeedyGenerationSettings.GetDefaultCriticalHeight();

            var criticalHtml = string.Empty;

            // If the setting is turned on to so that this is a public facing environment, then critical HTML can be generated via the hosted Node application on a separate URL.
            if (SpeedyGenerationSettings.IsPublicFacingEnvironment()
                || SpeedyGenerationSettings.UseLocalCriticalCssGenerator())
            {
                criticalHtml = _criticalGenerationGateway.GenerateCritical(presentUrl, width, height, true);
            }

            item.Fields[SpeedyConstants.Fields.CriticalCss].Value = criticalHtml;
        }
    }
}