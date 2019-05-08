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
using Sitecore.Sites;
using Sitecore.Web;

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
            if ((item.Database != null && String.Compare(item.Database.Name, this.Database) != 0) || item.Name == "__Standard Values")
            {
                return;
            }

            if (item != null && item.InheritsFrom(SpeedyConstants.TemplateIDs.SpeedyPageTemplateID))
            {
                // either the flag to generate on each page load is set or the CSS field is empty
                bool isEmpty = !item.Fields[SpeedyConstants.Fields.CriticalCSS].HasValue || string.IsNullOrWhiteSpace(item.Fields[SpeedyConstants.Fields.CriticalCSS].Value);
                var shouldGenerate = SpeedyGenerationSettings.ShouldRegenerateOnEachSave() || isEmpty;

                // If speedy is enabled for this page and should we generate the CSS
                if (item.IsEnabled(SpeedyConstants.Fields.SpeedyEnabled) && shouldGenerate)
                {
                    ICriticalGenerationGateway criticalGateway = null;

                    string presentUrl = GetUrlForContextSite(item) + $"?{SpeedyConstants.ByPass.ByPassParameter}=true";

                    string width = item.Fields[SpeedyConstants.Fields.CriticalViewPortWidth].Value;
                    string height = item.Fields[SpeedyConstants.Fields.CriticalViewPortHeight].Value;

                    string criticalHtml = string.Empty;
                    if(SpeedyGenerationSettings.IsPublicFacingEnvironment())
                    {
                        criticalGateway = new CriticalGenerationGateway();
                        criticalHtml = criticalGateway.GenerateCritical(presentUrl, width, height);
                    }else
                    {
                        criticalGateway = new CriticalGenerationNodeGateway();
                        criticalHtml = criticalGateway.GenerateCritical(presentUrl, width, height);
                    }
                        
                    item.Fields[SpeedyConstants.Fields.CriticalCSS].Value = criticalHtml;
                }
            }

        }

        public static string GetUrlForContextSite(Item item)
        {
            SiteInfo siteInfo = SiteContextFactory.Sites
            .Where(s => s.RootPath != "" & item.Paths.Path.StartsWith(s.RootPath, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(s => s.RootPath.Length)
            .FirstOrDefault();
            var site = SiteContext.GetSite(siteInfo.Name);

            using (var siteContextSwitcher = new SiteContextSwitcher(site))
            {
                var urlOptions = LinkManager.GetDefaultUrlOptions();
                urlOptions.AlwaysIncludeServerUrl = true;
                urlOptions.ShortenUrls = true;
                urlOptions.SiteResolving = true;
                return LinkManager.GetItemUrl(item, urlOptions);
            }
            return string.Empty;
        }
    }
}