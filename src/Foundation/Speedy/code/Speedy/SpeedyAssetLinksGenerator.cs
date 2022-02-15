using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Foundation.Speedy.Model;
using Sitecore.Foundation.Speedy.Settings;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.Pipelines;
using Sitecore.XA.Foundation.Theming;
using Sitecore.XA.Foundation.Theming.Bundler;
using Sitecore.XA.Foundation.Theming.Configuration;
using Sitecore.XA.Foundation.Theming.EventHandlers;
using Sitecore.XA.Foundation.Theming.Extensions;
using Sitecore.XA.Foundation.Theming.Pipelines.AssetService;
using static Sitecore.Foundation.Speedy.SpeedyConstants;
using Sitecore.Mvc.Presentation;
using System.Text;
using System;
using Sitecore.Mvc.Extensions;
using System.Web.Caching;
using Sitecore.XA.Foundation.SitecoreExtensions.Repositories;
using System.Net;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Sitecore.Foundation.Speedy.Model.Filters;

namespace Sitecore.Foundation.Speedy.Speedy
{
    public class SpeedyAssetLinksGenerator : AssetLinksGenerator
    {
        private readonly AssetConfiguration _configuration;

        public SpeedyAssetLinksGenerator() : base()
        {
            _configuration = AssetConfigurationReader.Read();
        }

        public void AddUrlIncludeSpeedy(UrlInclude urlInclude, SpeedyAssetLinks result)
        {
            if (urlInclude.Type == AssetType.Script)
            {
                result.Scripts.Add($"{urlInclude.Url}");
            }
            else
            {
                result.PlainStyles.Add(urlInclude.Url);
                result.Styles.Add($"<link async=\"true\" href=\"{urlInclude.Url}\" rel=\"stylesheet\" />");
            }
        }

        public static SpeedyLayoutModel GetSpeedyLayoutModel()
        {
            string text = GetPageKey(HttpContext.Current.Request.Url.AbsolutePath) + "speedysettings";
            SpeedyLayoutModel speedyModel = HttpContext.Current.Cache[text] as SpeedyLayoutModel;

            if (SpeedyGenerationSettings.IsDebugModeEnabled())
                speedyModel = null;

            if (speedyModel != null)
            {
                return speedyModel;
            }

            SpeedyLayoutModel model = new SpeedyLayoutModel();
            model.CacheKey = text;
            model.SpeedyEnabled = SpeedyGenerationSettings.IsSpeedyEnabledForPage(Sitecore.Context.Item);
            model.SpeedyJsEnabled = false;
            model.SpeedyCssEnabled = false;
            bool isOnePassCookieEnabledAndPresent = SpeedyGenerationSettings.IsOnePassCookieEnabled(Sitecore.Context.Item) && !string.IsNullOrWhiteSpace(HttpContext.Current.Request[SpeedyConstants.CookieNames.OnePassCookieName]);
            model.ByPassNotDetected = string.IsNullOrWhiteSpace(HttpContext.Current.Request[SpeedyConstants.ByPass.ByPassParameter]) && !isOnePassCookieEnabledAndPresent;

            if (!model.ByPassNotDetected)
                model.SpeedyEnabled = false;
            
            model.VanillaJavasriptAllLoads = GetVanillaJavascriptAllLoades();

            if (model.SpeedyEnabled && model.ByPassNotDetected)
            {
                BuildSpeedy(model);
            }
            else
            {
                model.AssetLinks = AssetLinksGenerator.GenerateLinks(new ThemesProvider());
            }

            return model;
        }

        private static void BuildSpeedy(SpeedyLayoutModel model)
        {
            var speedyLinks = GenerateDeferedLinks(new ThemesProvider());
            model.AssetLinks = speedyLinks;
            model.VanillaJavasript = GetVanillaJavascript();

            model.SpeedyJsEnabled = SpeedyGenerationSettings.IsCriticalJavascriptEnabledAndPossible(Sitecore.Context.Item);
            model.SpeedyCssEnabled = SpeedyGenerationSettings.IsCriticalStylesEnabled(Sitecore.Context.Item);

            LoadCssIntoModel(model, speedyLinks);
        }

        private static void LoadCssIntoModel(SpeedyLayoutModel model, SpeedyAssetLinks speedyLinks)
        {
            string largeCiticalCssBlockCacheKey = $"speedy-entire-css-critical-page-block-{Sitecore.Context.Item.ID}";
            string largeCriticalCssBlockCache = HttpContext.Current.Cache[largeCiticalCssBlockCacheKey] as string;

            if (SpeedyGenerationSettings.IsDebugModeEnabled())
                largeCriticalCssBlockCache = null;

            if (!string.IsNullOrWhiteSpace(largeCriticalCssBlockCache))
            {
                model.CriticalHtml = largeCriticalCssBlockCache;
            }
            else
            {
                model.CriticalHtml = BuildEntireCssBlock(speedyLinks);
                CacheObject(largeCiticalCssBlockCacheKey, model.CriticalHtml, GetDependencies(null));
            }
            model.SpecialCaseCriticalCss = Sitecore.Context.Item.Fields[SpeedyConstants.Fields.SpecialCaseCriticalCss].Value;
        }

        private static string GetVanillaJavascript()
        {
            string url = Sitecore.Context.Item.Fields[SpeedyConstants.Fields.MobileCriticalJavascript].Value;
            if (!string.IsNullOrWhiteSpace(url))
                return DownloadCssFile(url, true);
            else
                return string.Empty;
        }

        private static string GetVanillaJavascriptAllLoades()
        {
            string url = Sitecore.Context.Item.Fields[SpeedyConstants.Fields.EveryLoadVanillaJavscriptFile].Value;
            if (!string.IsNullOrWhiteSpace(url))
                return DownloadCssFile(url, true);
            else
                return string.Empty;
        }

        /// <summary>
        /// Throwing the entire CSS block inside the Critical head block is not the most streamlined method. Ideally the web team would use the NPM Critical
        /// package. However This is fallback for the lazy web team that ultimately just won't do that.
        /// Loop over the CSS assets, download the contents and append to one big Critical block
        /// </summary>
        /// <param name="assetLinks"></param>
        /// <returns></returns>
        private static string BuildEntireCssBlock(SpeedyAssetLinks assetLinks)
        {
            StringBuilder entireCriticalBlock = new StringBuilder();

            // Lookup the filters
            var nameValueListString = SpeedyGenerationSettings.GetGlobalSettingsItemFromContext()[SpeedyConstants.GlobalSettings.Fields.CSSFilter];

            //Converts the string to NameValueCollection
            System.Collections.Specialized.NameValueCollection nameValueList = Sitecore.Web.WebUtil.ParseUrlParameters(nameValueListString);

            foreach (var style in assetLinks.PlainStyles)
            {
                ApplyStyleFile(nameValueList, entireCriticalBlock, style);
            }

            entireCriticalBlock = entireCriticalBlock.Replace("font-family:", "font-display:swap;font-family:");

            return entireCriticalBlock.ToString();
        }

        private static void ApplyStyleFile(NameValueCollection nameValueList, StringBuilder entireCriticalBlock, string style)
        {
            string uri = style.ValueOrEmpty();
            var cssContents = new StringBuilder(DownloadCssFile(uri));
            string[] parts = uri.Split('/');
            if (cssContents.Length > 10 && parts.Length > 5)
            {
                cssContents = ApplyGlobalFilters(uri, cssContents, nameValueList);

                string replacementSection = $"/-/media/Themes/{parts[4]}/{parts[5]}/fonts/";

                // Yes as crazy as it seems all three of the following combos were in the habitat example site. So lets deal with all the cases
                cssContents = cssContents.Replace("url('../fonts/", $"url('{replacementSection}"); // With a '
                cssContents = cssContents.Replace("url(\"../fonts/", $"url(\"{replacementSection}"); // With a "
                cssContents = cssContents.Replace("url(../fonts/", $"url({replacementSection}");  // Without a '

                entireCriticalBlock = entireCriticalBlock.Append(cssContents);
            }
        }

        private static StringBuilder ApplyGlobalFilters(string uri, StringBuilder cssContents, System.Collections.Specialized.NameValueCollection nameValueList)
        {
            try
            {
                var withoutTail = uri.Split('?')[0];
                var filterMatch = nameValueList[withoutTail.ToLower()];
                if (filterMatch != null)
                {
                    var filters = JsonConvert.DeserializeObject<Filters>(filterMatch.ValueOrEmpty());
                    foreach (var filter in filters.FilterList)
                    {
                        string contents = cssContents.ToString();
                        if (contents.Contains(filter.Start) && contents.Contains(filter.End))
                        {
                            int startIndex = contents.IndexOf(filter.Start);
                            int endIndex = contents.IndexOf(filter.End);
                            cssContents = cssContents.Remove(startIndex, endIndex - startIndex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return cssContents;
        }

        private static string DownloadCssFile(string url, bool acceptJs = false)
        {
            string cssFileCacheKey = $"speedy-external-css-{url}";
            string cssFileCache = HttpContext.Current.Cache[cssFileCacheKey] as string;

            if (SpeedyGenerationSettings.IsDebugModeEnabled())
                cssFileCache = null;

            if (!string.IsNullOrWhiteSpace(cssFileCache))
            {
                return cssFileCache;
            }
            else
            {
                var uri = HttpContext.Current.Request.Url;
                var host = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;

                // or automatically deserialize result
                // return content type is sniffed but can be explicitly set via RestClient.AddHandler();
                try
                {
                    WebClient client = new WebClient();
                    string reply = client.DownloadString(host + url);

                    // The below is commented out, and originally slipped through to master by mistake.
                    // If your testing the module in development and don't have a valid SSL certificate, you might need to comment it in.
//#if DEBUG
//                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
//#endif
                    if (!string.IsNullOrWhiteSpace(reply))
                        CacheObject(cssFileCacheKey, reply, GetDependencies(null));
                    return reply;
                }
                catch (Exception ex)
                {
                    Diagnostics.Log.Error("Download CSS SpeedyAssetLinksGenerator", ex);
                }
            }
            return string.Empty;
        }

        public virtual SpeedyAssetLinks GenerateSpeedyAssetLinks(IThemesProvider themesProvider)
        {
            AssetsArgs assetsArgs = new AssetsArgs();
            CorePipeline.Run("assetService", assetsArgs);
            string text = GenerateCacheKey(assetsArgs.GetHashCode()) + "speedylinks-" + Sitecore.Context.Item.ID;
            SpeedyAssetLinks assetLinks = HttpContext.Current.Cache[text] as SpeedyAssetLinks;

            if (SpeedyGenerationSettings.IsDebugModeEnabled())
                assetLinks = null;

            if (assetLinks == null || _configuration.RequestAssetsOptimizationDisabled)
            {
                assetLinks = new SpeedyAssetLinks();
                if (!assetsArgs.AssetsList.Any())
                {
                    return assetLinks;
                }
                assetsArgs.AssetsList = (from a in assetsArgs.AssetsList
                                         orderby a.SortOrder
                                         select a).ToList();
                foreach (AssetInclude assets in assetsArgs.AssetsList)
                {
                    if (assets is ThemeInclude)
                    {
                        AddThemeIncludeSpeedy(assets as ThemeInclude, assetLinks, themesProvider);
                    }
                    else if (assets is UrlInclude)
                    {
                        AddUrlIncludeSpeedy(assets as UrlInclude, assetLinks);
                    }
                    else if (assets is PlainInclude)
                    {
                        AddPlainInclude(assets as PlainInclude, assetLinks);
                    }
                }
                
                CacheLinks(text, assetLinks, GetDependencies(this.DatabaseRepository));
            }
            return assetLinks;
        }

        protected virtual void AddThemeIncludeSpeedy(ThemeInclude themeInclude, SpeedyAssetLinks result, IThemesProvider themesProvider)
        {
            Item item = themeInclude.Theme;
            if (item == null && !themeInclude.ThemeId.IsNull)
            {
                item = ContentRepository.GetItem(themeInclude.ThemeId);
            }
            if (item != null)
            {
                Log.Debug($"Starting optimized files generation process for {item.Name} with following configuration {_configuration}");
                IList<Item> allThemes = ServiceLocator.ServiceProvider.GetService<IThemingContext>().GetAllThemes(item);
                GetLinksSpeedy(allThemes.FilterBaseThemes(), _configuration.ScriptsMode, _configuration.StylesMode, result);
                GetLinksSpeedy(themesProvider.GetThemes(item, allThemes), _configuration.ScriptsMode, _configuration.StylesMode, result);
            }
        }

        protected virtual void GetLinksSpeedy(IEnumerable<Item> allThemes, AssetServiceMode scriptsMode, AssetServiceMode stylesMode, SpeedyAssetLinks result)
        {
            foreach (Item allTheme in allThemes)
            {
                AssetLinks assetLinks = new AssetLinks();
                GetScriptLinks(allTheme, scriptsMode, assetLinks);
                GetStylesLinks(allTheme, stylesMode, assetLinks);
                foreach (string item in from link in assetLinks.Styles
                                        select $"<link async=\"true\" href=\"{link}\" rel=\"stylesheet\" />")
                {
                    var cssFileUri = item.Replace($"<link async=\"true\" href=\"", string.Empty).Replace($"\" rel=\"stylesheet\" />", string.Empty);
                    var prefetchInclude = item.Replace($"async=\"true\"", $"async=\"true\" rel=\"prefetch\"");
                    result.PlainStyles.Add(cssFileUri);
                    result.PrefetchStyles.Add(prefetchInclude);
                    result.Styles.Add(item);
                }
                foreach (string item2 in from link in assetLinks.Scripts
                                         select link)
                {
                    result.Scripts.Add(item2);
                }
            }
        }

        public static SpeedyAssetLinks GenerateDeferedLinks(IThemesProvider themesProvider)
        {
            if (AssetContentRefresher.IsPublishing() || IsAddingRendering())
            {
                return new SpeedyAssetLinks();
            }
            var assetsGenerator = new SpeedyAssetLinksGenerator();
            var links = assetsGenerator.GenerateSpeedyAssetLinks(themesProvider);
            var linkSpeedy = new SpeedyAssetLinks(links);

            var AreScriptsDeferred = SpeedyGenerationSettings.IsCriticalJavascriptEnabledAndPossible(Sitecore.Context.Item);
            if (AreScriptsDeferred)
            {
                string deferredSriptsCacheKey = $"speedy-deferred-page-scripts-{Sitecore.Context.Item.ID}";
                string preloadSriptsCacheKey = $"speedy-preload-page-scripts-{Sitecore.Context.Item.ID}";
                string deferredSriptsCache = HttpContext.Current.Cache[deferredSriptsCacheKey] as string;
                string preloadSriptsCache = HttpContext.Current.Cache[preloadSriptsCacheKey] as string;

                if (SpeedyGenerationSettings.IsDebugModeEnabled())
                {
                    deferredSriptsCache = null;
                    preloadSriptsCache = null;
                }

                if (!string.IsNullOrWhiteSpace(deferredSriptsCache) && !string.IsNullOrWhiteSpace(preloadSriptsCache))
                {
                    linkSpeedy.ClientScriptsRendered = deferredSriptsCache;
                    linkSpeedy.ClientScriptsPreload = preloadSriptsCache;
                }
                else
                {
                    assetsGenerator.GenerateSpeedyScripts(linkSpeedy);
                    CacheObject(deferredSriptsCacheKey, linkSpeedy.ClientScriptsRendered, GetDependencies(null));
                    CacheObject(preloadSriptsCacheKey, linkSpeedy.ClientScriptsPreload, GetDependencies(null));
                }
            }
            else
            {
                var linksa = AssetLinksGenerator.GenerateLinks(new ThemesProvider());
                linkSpeedy.Scripts = linksa.Scripts;
            }
            return linkSpeedy;
        }

        private void GenerateSpeedyScripts(SpeedyAssetLinks assetLinks)
        {
            assetLinks.ClientScriptsRendered = "";

            string result = string.Empty;
            string resultPreloaded = string.Empty;
            int count = 0;
            foreach (var scripts in assetLinks.Scripts)
            {
                string comma = ",";
                if (count == assetLinks.Scripts.Count)
                    comma = string.Empty;

                result += $"'{scripts}'{comma}";
                resultPreloaded += $"<link rel=\"prefetch\" href=\"{scripts}\" as=\"script\">";
                count++;
            }

            var phase = $"var clientScripts = [{result}]\r";

            assetLinks.ClientScriptsRendered += phase;
            assetLinks.ClientScriptsPreload = resultPreloaded;
        }

        public static string GetPageKey(string url)
        {
            var key = url;
            string contextItemId = string.Empty;
            if (RenderingContext.Current.ContextItem != null)
            {
                contextItemId = RenderingContext.Current.PageContext?.Item?.ID.ToShortID().ToString();
            }
            if (key == "/")
            {
                key = "homepage-" + contextItemId;
            }
            else
                key = url + "-" + contextItemId;
            return KeyFilter(key);
        }

        public static string KeyFilter(string value)
        {
            return value.Replace(" ", "_").Replace("/", "_").ToLower();
        }

        protected static void CacheObject(string cacheKey, object result, string[] dependencyKeys)
        {
            HttpContext.Current.Cache.Add(cacheKey, result, new CacheDependency(null, dependencyKeys), DateTime.UtcNow.AddMinutes(10.0), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
        }

        protected static string[] GetDependencies(IDatabaseRepository databaseRepository)
        {
            if(databaseRepository == null)
                databaseRepository = ServiceProviderServiceExtensions.GetService<IDatabaseRepository>(ServiceLocator.ServiceProvider);
            return (databaseRepository.GetContentDatabase().Name.ToLower() == GlobalSettings.Database.Master) ? AssetContentRefresher.MasterCacheDependencyKeys : AssetContentRefresher.WebCacheDependencyKeys;
        }
    }
}