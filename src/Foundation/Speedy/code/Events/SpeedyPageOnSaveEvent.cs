using Sitecore.Data.Items;
using Sitecore.Events;
using Sitecore.Foundation.Speedy.Extensions;
using Sitecore.Foundation.Speedy.Services;
using Sitecore.Foundation.Speedy.Settings;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using System;

namespace Sitecore.Foundation.Speedy.Events
{
    public class SpeedyPageOnSaveEvent
    {
        public string Database
        {
            get;
            set;
        }

        public void OnItemSaving(object sender, EventArgs args)
        {
            var item = Event.ExtractParameter(args, 0) as Item;

            if (item.Name == "__Standard Values" || !item.IsSpeedyEnabledForPage())   // Nothing to see here lets exit quickly
                return;

            Sitecore.Diagnostics.Log.Info("SpeedyPageOnSaveEvent running", this);

            var shouldGenerate = SpeedyGenerationSettings.ShouldRegenerateOnEachSave();

            // If speedy is enabled for this page and should we generate the CSS
            if (shouldGenerate)
            {
                UpdateCritical(item);
            }
        }

        public void UpdateCritical(Item item)
        {
            // Only act if within the master database
            if ((item.Database != null && String.Compare(item.Database.Name, this.Database) != 0) || item.Name == "__Standard Values")
            {
                return;
            }

            if (item != null && item.InheritsFrom(SpeedyConstants.TemplateIDs.SpeedyPageTemplateId))
            {
                // If speedy is enabled for this page and should we generate the CSS
                if (item.IsEnabled(SpeedyConstants.Fields.SpeedyEnabled))
                {
                    ICriticalGenerationGateway criticalGateway = null;

                    string presentUrl = item.GetUrlForContextSite() + $"?{SpeedyConstants.ByPass.ByPassParameter}=true";

                    string width = item.Fields[SpeedyConstants.Fields.CriticalViewPortWidth].Value;
                    string height = item.Fields[SpeedyConstants.Fields.CriticalViewPortHeight].Value;

                    if (string.IsNullOrWhiteSpace(width))
                        width = SpeedyGenerationSettings.GetDefaultCriticalWidth();
                    if (string.IsNullOrWhiteSpace(height))
                        height = SpeedyGenerationSettings.GetDefaultCriticalHeight();

                    string criticalHtml = string.Empty;

                    // If the setting is turned on to so that this is a public facing environment, then critical HTML can be generated via the hosted Node application on a seperate URL.
                    if (SpeedyGenerationSettings.IsPublicFacingEnvironment())
                    {
                        criticalGateway = new CriticalGenerationGateway();
                        criticalHtml = criticalGateway.GenerateCritical(presentUrl, width, height, fontReplace: true);
                    }

                    item.Fields[SpeedyConstants.Fields.CriticalCss].Value = criticalHtml;
                }
            }
        }
    }
}