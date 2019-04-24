using Sitecore.Data.Items;
using Sitecore.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using System.Web;

namespace Site.Foundation.PageSpeed.Events
{
    public class SpeedyPageOnSaveEvent
    {
        public void OnItemSaved(object sender, EventArgs args)
        {
            var item = Event.ExtractParameter(args, 0) as Item;

            if (item != null && item.InheritsFrom(SpeedyConstants.TemplateIDs.SpeedyPageTemplateID))
            {
                if(IsEnabled(item, SpeedyConstants.Fields.SpeedyEnabled))
                {

                }
            }
        }

        public bool IsEnabled(Item item, string fieldName)
        {
            return item.Fields[fieldName].HasValue && item.Fields[fieldName].Value == "1";
        }
    }
}