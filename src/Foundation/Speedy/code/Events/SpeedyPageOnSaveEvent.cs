using System;
using System.Linq;
using Site.Foundation.Speedy.Extensions;
using Site.Foundation.Speedy.Services;
using Site.Foundation.Speedy.Settings;
using Sitecore.Data.Items;
using Sitecore.Events;
using Sitecore.Links;
using Sitecore.Sites;
using Sitecore.Web;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;

namespace Site.Foundation.Speedy.Events
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

            if (item != null && item.InheritsFrom(SpeedyConstants.TemplateIDs.SpeedyPageTemplateId))
            {
                // either the flag to generate on each page load is set or the CSS field is empty
                bool isEmpty = !item.Fields[SpeedyConstants.Fields.CriticalCss].HasValue || string.IsNullOrWhiteSpace(item.Fields[SpeedyConstants.Fields.CriticalCss].Value);
                var shouldGenerate = SpeedyGenerationSettings.ShouldRegenerateOnEachSave() || isEmpty;

                // If speedy is enabled for this page and should we generate the CSS
                if (item.IsEnabled(SpeedyConstants.Fields.SpeedyEnabled) && shouldGenerate)
                {
                    ICriticalGenerationGateway criticalGateway = null;

                    string presentUrl = item.GetUrlForContextSite() + $"?{SpeedyConstants.ByPass.ByPassParameter}=true";

                    string width = item.Fields[SpeedyConstants.Fields.CriticalViewPortWidth].Value;
                    string height = item.Fields[SpeedyConstants.Fields.CriticalViewPortHeight].Value;

                    string criticalHtml = string.Empty;

                    // If the setting is turned on to so that this is a public facing environment, then critical HTML can be generated via the hosted Node application on a seperate URL.
                    if(SpeedyGenerationSettings.IsPublicFacingEnvironment())
                    {
                        criticalGateway = new CriticalGenerationGateway();
                        criticalHtml = criticalGateway.GenerateCritical(presentUrl, width, height);
                    }
                        
                    item.Fields[SpeedyConstants.Fields.CriticalCss].Value = criticalHtml;
                }
            }
        }
    }
}