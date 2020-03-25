using System;
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

namespace Sitecore.Foundation.ImageCompression.Scheduler
{
    public class SyncImages
    {
        public virtual void RunSync()
        {
            if (!ImageCompressionSettings.IsImageCompressionScheduledTaskEnabled())
                return;

            var imageCompressionService = ServiceLocator.ServiceProvider.GetService<IImageCompressionService>();

            Sitecore.Diagnostics.Log.Info("Commencing the Image Compression task", this);

            using (var context = ContentSearchManager.GetIndex(ImageCompression.ImageCompressionConstants.GlobalSettings.Index.Master).CreateSearchContext())
            {
                foreach (var result in GetUnprocessedImages(context.Index))
                {
                    try
                    {
                        if (result.Name == "__Standard Values")
                            continue;
                        Sitecore.Diagnostics.Log.Info($"STARTED - Image Compression task processing {result.ID} - {result.Name}", this);
                        imageCompressionService.CompressImage(result);
                        Sitecore.Diagnostics.Log.Info($"COMPLETED - Image Compression task processing {result.ID} - {result.Name}", this);
                    }
                    catch(Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error($"FAILED - Image Compression task {result.ID} - {result.Name} - {ex.Message} \n\r {ex.InnerException}", this);
                    }

                    if(!imageCompressionService.ShouldContinue)
                    {
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Requires the following field to be patched in: <field fieldName="_templates"                 returnType="string"      type="Sitecore.ContentSearch.ComputedFields.AllTemplates, Sitecore.ContentSearch" deep="true" includeStandardTemplate="false" />
        /// </summary>
        /// <param name="index">The search index to search within.</param>
        /// <returns></returns>
        protected IEnumerable<Item> GetUnprocessedImages(ISearchIndex index)
        {
            using (var searchContext = index.CreateSearchContext())
            {
                var imageResults = searchContext.GetQueryable<AllTemplatesSearchResultItem>().Where(x =>
                    x.TemplateId == ImageCompressionConstants.TemplateIDs.UnversionedImageTemplateId
                    || x.TemplateId == (ImageCompressionConstants.TemplateIDs.UnversionedJpegImageTemplateId)
                    || x.TemplateId == (ImageCompressionConstants.TemplateIDs.VersionedImageTemplateId)
                    || x.TemplateId == (ImageCompressionConstants.TemplateIDs.VersionedJpegImageTemplateId)
                );
                if (!imageResults.Any())
                    return new List<Item>();

                var sitecoreItems = imageResults.Select(x => x.GetItem()).ToList()
                    .Where(y => !y.Fields[ImageCompressionSettings.GetInformationField()].Value.Contains(ImageCompressionConstants.Messages.OPTIMISED_BY) );
                var results = sitecoreItems.ToList();

                Diagnostics.Log.Info($"Found {results.Count} images to compress", this);

                return results;
            }
        }
    }
}
