using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Data.Items;
using Sitecore.Foundation.Speedy.Events;
using Sitecore.Foundation.Speedy.Extensions;
using Sitecore.Foundation.Speedy.Settings;
using static Sitecore.Foundation.Speedy.SpeedyConstants;

namespace Sitecore.Foundation.Speedy.Scheduler
{
    public class SyncCritical
    {
        public virtual void RunSync()
        {
            if (!SpeedyGenerationSettings.IsPublicFacingEnvironment())
                return;

            var speedyPageEvt = new SpeedyPageOnSaveEvent();
            speedyPageEvt.Database = GlobalSettings.Database.Master;

            using (var context = ContentSearchManager.GetIndex("sitecore_master_index").CreateSearchContext())
            {            
                foreach (var result in GetSpeedyPagesByTemplate(context.Index))
                {
                    if(result.IsSpeedyEnabledForPage() && result.IsCriticalStylesEnabledAndPossible())
                    {
                        speedyPageEvt.UpdateCritical(result);
                    }
                }
            }
        }

        /// <summary>
        /// Requires the following field to be patched in: <field fieldName="_templates"                 returnType="string"      type="Sitecore.ContentSearch.ComputedFields.AllTemplates, Sitecore.ContentSearch" deep="true" includeStandardTemplate="false" />
        /// </summary>
        /// <param name="index">The search index to search within.</param>
        /// <returns></returns>
        protected IEnumerable<Item> GetSpeedyPagesByTemplate(ISearchIndex index)
        {                                                                      
            using (var searchContext = index.CreateSearchContext())
            {
                var speedyPages = searchContext.GetQueryable<AllTemplatesSearchResultItem>().Where(x => x.ItemBaseTemplates.Contains(SpeedyConstants.TemplateIDs.SpeedyPageTemplateId));
                if (!speedyPages.Any())
                    return new List<Item>();

                var sitecoreItems = speedyPages.Select(x => x.GetItem()).ToList().Where(y => y.IsEnabled(SpeedyConstants.Fields.SpeedyEnabled));
                return sitecoreItems.ToList();
            }
        }
    }
}
