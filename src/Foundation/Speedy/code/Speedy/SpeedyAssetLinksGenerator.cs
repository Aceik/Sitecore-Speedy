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

namespace Sitecore.Foundation.Speedy.Speedy
{
    public class SpeedyAssetLinksGenerator : AssetLinksGenerator
    {
        private readonly AssetConfiguration _configuration;

        public SpeedyAssetLinksGenerator() : base()
        {
            _configuration = AssetConfigurationReader.Read();
        }

        public void AddUrlIncludeSpeedy(UrlInclude urlInclude, AssetLinks result)
        {
            if (urlInclude.Type == AssetType.Script)
            {
                result.Scripts.Add($"{urlInclude.Url}");
            }
            else
            {
                result.Styles.Add($"<link async=\"true\" href=\"{urlInclude.Url}\" rel=\"stylesheet\" />");
            }
        }

        public static SpeedyLayoutModel GetSpeedyLayoutModel()
        {
            string text = GetPageKey(HttpContext.Current.Request.Url.AbsolutePath) + "speedysettings";
            SpeedyLayoutModel speedyModel = HttpContext.Current.Cache[text] as SpeedyLayoutModel;
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

            if (model.SpeedyEnabled && model.ByPassNotDetected)
            {
                model.AssetLinks = GenerateDeferedLinks(new ThemesProvider());
                model.SpeedyJsEnabled = SpeedyGenerationSettings.IsCriticalJavascriptEnabledAndPossible(Sitecore.Context.Item);
                model.SpeedyCssEnabled = SpeedyGenerationSettings.IsCriticalStylesEnabledAndPossible(Sitecore.Context.Item);

                if (model.SpeedyCssEnabled)
                {
                    model.CriticalHtml = Sitecore.Context.Item.Fields[SpeedyConstants.Fields.CriticalCss].Value;
                    model.SpecialCaseCriticalCss = Sitecore.Context.Item.Fields[SpeedyConstants.Fields.SpecialCaseCriticalCss].Value;
                }
            }
            else
            {
                model.AssetLinks = AssetLinksGenerator.GenerateLinks(new ThemesProvider());
            }

            return model;
        }

        public virtual SpeedyAssetLinks GenerateSpeedyAssetLinks(IThemesProvider themesProvider)
        {
            AssetsArgs assetsArgs = new AssetsArgs();
            CorePipeline.Run("assetService", assetsArgs);
            string text = GenerateCacheKey(assetsArgs.GetHashCode()) + "speedylinks";
            SpeedyAssetLinks assetLinks = HttpContext.Current.Cache[text] as SpeedyAssetLinks;
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
                
                CacheLinks(text, assetLinks, (DatabaseRepository.GetContentDatabase().Name.ToLower() == GlobalSettings.Database.Master) ? AssetContentRefresher.MasterCacheDependencyKeys : AssetContentRefresher.WebCacheDependencyKeys);
            }
            return assetLinks;
        }

        protected virtual void AddThemeIncludeSpeedy(ThemeInclude themeInclude, AssetLinks result, IThemesProvider themesProvider)
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

        protected virtual void GetLinksSpeedy(IEnumerable<Item> allThemes, AssetServiceMode scriptsMode, AssetServiceMode stylesMode, AssetLinks result)
        {
            foreach (Item allTheme in allThemes)
            {
                AssetLinks assetLinks = new AssetLinks();
                GetScriptLinks(allTheme, scriptsMode, assetLinks);
                GetStylesLinks(allTheme, stylesMode, assetLinks);
                foreach (string item in from link in assetLinks.Styles
                                        select $"<link async=\"true\" href=\"{link}\" rel=\"stylesheet\" />")
                {
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
            assetsGenerator.GenerateSpeedyScripts(linkSpeedy);
            return linkSpeedy;
        }

        private void GenerateSpeedyScripts(SpeedyAssetLinks assetLinks)
        {
            assetLinks.ClientScriptsRendered = "";

            string result = string.Empty;
            int count = 0;
            foreach (var scripts in assetLinks.Scripts)
            {
                string comma = ",";
                if (count == assetLinks.Scripts.Count)
                    comma = string.Empty;

                result += $"'{scripts}'{comma}";
                count++;
            }

            var phase = $"var clientScripts = [{result}]\r";

            assetLinks.ClientScriptsRendered += phase;
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
    }
}