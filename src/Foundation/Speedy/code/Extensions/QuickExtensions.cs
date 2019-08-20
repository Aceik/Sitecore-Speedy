using System;
using System.Linq;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Sites;
using Sitecore.Web;

namespace Sitecore.Foundation.Speedy.Extensions
{
    public static class QuickExtensions
    {
        public static bool IsEnabled(this Item item, string fieldName)
        {
            return item.Fields[fieldName] != null && item.Fields[fieldName].Value == "1";
        }

        public static string GetUrlForContextSite(this Item item)
        {
            var sites = SiteContextFactory.Sites
                .Where(s => !string.IsNullOrWhiteSpace(s.RootPath) &
                            item.Paths.Path.StartsWith(s.RootPath, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(s => s.RootPath.Length).ToList();

            SiteInfo siteInfo = sites.FirstOrDefault();
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