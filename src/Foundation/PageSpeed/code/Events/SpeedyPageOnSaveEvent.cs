using Sitecore.Data.Items;
using Sitecore.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using System.Web;
using Site.Foundation.PageSpeed.Repositories;
using Sitecore.Links;
using Site.Foundation.PageSpeed.Extensions;
using Site.Foundation.PageSpeed.Settings;

namespace Site.Foundation.PageSpeed.Events
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
            Sitecore.Diagnostics.Log.Info("SpeedyPageOnSaveEvent running", this);
            var item = Event.ExtractParameter(args, 0) as Item;

            // Only act if within the master database
            if (item.Database != null && String.Compare(item.Database.Name, this.Database) != 0)
            {
                return;
            }

            if (item != null && item.InheritsFrom(SpeedyConstants.TemplateIDs.SpeedyPageTemplateID))
            {
                // either the flag to generate on each page load is set or the CSS field is empty
                var shouldGenerate = SpeedyGenerationSettings.ShouldRegenerateOnEachSave() || !item.Fields[SpeedyConstants.Fields.CriticalCSS].HasValue;

                // If speedy is enabled for this page and should we generate the CSS
                if (item.IsEnabled(SpeedyConstants.Fields.SpeedyEnabled) && shouldGenerate)
                {
                    ICriticalGenerationGateway criticalGateway = new CriticalGenerationGateway();

                    string presentUrl = LinkManager.GetItemUrl(item, new UrlOptions { AlwaysIncludeServerUrl = true, ShortenUrls = true }) + $"?{SpeedyConstants.ByPass.ByPassParameter}=true";

                    string width = item.Fields[SpeedyConstants.Fields.CriticalViewPortWidth].Value;
                    string height = item.Fields[SpeedyConstants.Fields.CriticalViewPortHeight].Value;

                    string criticalHtml = criticalGateway.GenerateCritical(presentUrl, width, height);

                    item.Fields[SpeedyConstants.Fields.CriticalCSS].Value = criticalHtml;
                }
            }
        }
    }
}