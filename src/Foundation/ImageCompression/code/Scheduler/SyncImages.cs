using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.ContentSearch;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Foundation.ImageCompression;
using Sitecore.Foundation.ImageCompression.Model;
using Sitecore.Foundation.ImageCompression.Services;
using Sitecore.Foundation.ImageCompression.Settings;

namespace Sitecore.Foundation.Speedy.Scheduler
{
    public class SyncImages
    {
        public virtual void RunSync()
        {
            if (!ImageCompressionSettings.IsImageCompressionScheduledTaskEnabled())
                return;

            var imageCompressionService = ServiceLocator.ServiceProvider.GetService<IImageCompressionService>();

            using (var context = ContentSearchManager.GetIndex(ImageCompression.ImageCompressionConstants.GlobalSettings.Index.Master).CreateSearchContext())
            {
                foreach (var result in GetSpeedyPagesByTemplate(context.Index))
                {
                    imageCompressionService.CompressImage(result);
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
                var speedyPages = searchContext.GetQueryable<AllTemplatesSearchResultItem>().Where(x => x.ItemBaseTemplates.Contains(ImageCompressionConstants.TemplateIDs.ImageTemplateId));
                if (!speedyPages.Any())
                    return new List<Item>();

                var sitecoreItems = speedyPages.Select(x => x.GetItem()).ToList()
                    .Where(y => !y.Fields[ImageCompressionSettings.GetInformationField()].Value.Contains(ImageCompressionConstants.Messages.OPTIMISED_BY) );
                return sitecoreItems.ToList();
            }
        }
    }
}
