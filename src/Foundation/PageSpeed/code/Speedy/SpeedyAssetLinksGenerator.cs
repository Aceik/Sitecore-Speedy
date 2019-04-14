using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.Pipelines;
using Sitecore.SecurityModel.License;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using Sitecore.XA.Foundation.Theming;
using Sitecore.XA.Foundation.Theming.Bundler;
using Sitecore.XA.Foundation.Theming.Configuration;
using Sitecore.XA.Foundation.Theming.EventHandlers;
using Sitecore.XA.Foundation.Theming.Extensions;
using Sitecore.XA.Foundation.Theming.Pipelines.AssetService;

namespace Site.Foundation.PageSpeed.Speedy
{
    public class SpeedyAssetLinksGenerator : AssetLinksGenerator
    {
        private readonly AssetConfiguration _configuration;

        public SpeedyAssetLinksGenerator() : base()
        {
            _configuration = AssetConfigurationReader.Read();
        }

        public static SpeedyAssetLinks GenerateDeferedLinks(IThemesProvider themesProvider)
        {
            if (AssetContentRefresher.IsPublishing() || IsAddingRendering())
            {
                return new SpeedyAssetLinks();
            }
            return new SpeedyAssetLinksGenerator().GenerateSpeedyAssetLinks(themesProvider);
        }

        public virtual SpeedyAssetLinks GenerateSpeedyAssetLinks(IThemesProvider themesProvider)
        {
            if (!License.HasModule("Sitecore.SXA"))
            {
                HttpContext.Current.Response.Redirect($"{Settings.NoLicenseUrl}?license=Sitecore.SXA");
                return null;
            }
            AssetsArgs assetsArgs = new AssetsArgs();
            CorePipeline.Run("assetService", assetsArgs);
            string text = GenerateCacheKey(assetsArgs.GetHashCode());
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

                GenerateSpeedyScriptsJson(assetLinks);

                CacheLinks(text, assetLinks, (DatabaseRepository.GetContentDatabase().Name.ToLower() == "master") ? AssetContentRefresher.MasterCacheDependencyKeys : AssetContentRefresher.WebCacheDependencyKeys);
            }
            return assetLinks;
        }

        private void GenerateSpeedyScriptsJson(SpeedyAssetLinks assetLinks)
        { 
            //if (assetLinks.ClientScriptsDictionary == null)
            //{
            //    assetLinks.CoreLibrariesScripts = $"{{ }}";
            //    return;
            //}

            //if(assetLinks.ClientScriptsDictionary.Values.Any(x => x.IndexOf("Core-Libraries") > -1))
            //{
            //    var coreLibValue = assetLinks.ClientScriptsDictionary.FirstOrDefault(x => x.Value.IndexOf("Core-Libraries") > -1);
            //    assetLinks.CoreLibrariesScripts = $"{{ '{coreLibValue.Key}' : '{coreLibValue.Value}' }}";
            //    //assetLinks.CoreLibrariesScriptNames = $"['{coreLibValue.Key}']";
            //    assetLinks.ClientScriptsDictionary.Remove(coreLibValue.Key);
            //}

            //string result = string.Empty;
            //string scriptNames = string.Empty;
            //int count = 1;
            //foreach (var script in assetLinks.ClientScriptsDictionary)
            //{
            //    string comma = ",";
            //    if (count == assetLinks.ClientScriptsDictionary.Count)
            //       comma = string.Empty;

            //    result += $"'{script.Key}' : '{script.Value}'{comma}";
            //    scriptNames += $"'{script.Key}'{comma}";
            //    count++;
            //}

            assetLinks.ClientScriptsRendered = "";

            var phases = assetLinks.OrderedScripts.OrderBy(x => x.Key);
            foreach(var singlePhase in phases)
            {
                string result = string.Empty;
                int count = 0;
                foreach (var phaseScripts in singlePhase.Value)
                {
                    string comma = ",";
                    if (count == singlePhase.Value.Values.Count)
                        comma = string.Empty;

                    result += $"'{phaseScripts.Value.Url}'{comma}";
                    count++;
                }

                var phase = $"clientScripts.phase{singlePhase.Key} = [{result}]\r";

                assetLinks.ClientScriptsRendered += phase;
            }

            //assetLinks.ClientScriptsRendered += finish;

            //assetLinks.ClientScripts = $"{{ {result} }}";
            //assetLinks.ClientScriptNames = $"[ {scriptNames} ]";
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
                SpeedyAssetLinks assetLinks = new SpeedyAssetLinks();
                GetScriptLinksSpeedy(allTheme, scriptsMode, assetLinks);
                GetStylesLinks(allTheme, stylesMode, assetLinks);
                foreach (string item in from link in assetLinks.Styles select $"<link href=\"{link}\" rel=\"stylesheet\" async=\"true\" />")
                {
                    result.Styles.Add(item);
                }

                if(assetLinks.OrderedScripts == null || (!assetLinks.OrderedScripts.Any() && assetLinks.OrderedScripts.Any()))
                {
                    result.OrderedScripts = assetLinks.OrderedScripts;
                }
                else
                {
                    var assets = assetLinks.OrderedScripts.SelectMany(x => x.Value);
                    foreach (var orderdScript in assets.Select(x => x.Value))
                    {
                        AddOrMerge(result, orderdScript);
                    }
                }
            }
        }

        public void AddUrlIncludeSpeedy(UrlInclude urlInclude, SpeedyAssetLinks result)
        {
            if (urlInclude.Type == AssetType.Script)
            {
                //if (result.ClientScriptsDictionary == null)
                //    result.ClientScriptsDictionary = new Dictionary<string, string>();

                //result.ClientScriptsDictionary.Add(GetScriptName(urlInclude.Url), urlInclude.Url);
                result.Scripts.Add($"{urlInclude.Url}");
            }
            else
            {
                result.Styles.Add($"<link async=\"true\" href=\"{urlInclude.Url}\" rel=\"stylesheet\" />");
            }
        }

        public string GetScriptName(string url)
        {
            //var tokens = url.Split('/');
            //string fullToken = tokens[tokens.Length - 2];
            var jsIndex = url.IndexOf(".js");
            //return fullToken.Substring(0, jsIndex);
            return url.Substring(0, jsIndex).Replace("/", "_").Replace("-", "").Replace("__media_BaseThemes_", "").ToLower().Replace("_scripts", string.Empty);
        }

        public string GetTimestamps(string url)
        {
            var jsIndex = url.IndexOf(".js");
            return url.Substring(jsIndex);
        }

        protected virtual void GetScriptLinksSpeedy(Item theme, AssetServiceMode scriptsMode, SpeedyAssetLinks result)
        {
            if(scriptsMode == AssetServiceMode.Concatenate || scriptsMode == AssetServiceMode.ConcatenateAndMinify)
            {
                if(result.OrderedScripts == null)
                    result.OrderedScripts = new Dictionary<int, Dictionary<string, SpeedyAssetLink>>();
            }

            if(scriptsMode == AssetServiceMode.Disabled)
            {
                foreach (string item in QueryAssets(theme, "./Scripts//*[@Extension='js']"))
                {
                    result.Scripts.Add(item);
                }
            }
            else if(scriptsMode == AssetServiceMode.Concatenate || scriptsMode == AssetServiceMode.ConcatenateAndMinify)
            {
                string fileNamePrefix = string.Empty;
                if (scriptsMode == AssetServiceMode.ConcatenateAndMinify)
                    fileNamePrefix = "-min";

                var optimizedItemLink2 = GetOptimizedItemLinkSpeedy(theme, OptimizationType.Scripts, scriptsMode, "./Scripts//*[@@templateid='{0}' and @@name='{1}']", ("optimized" + fileNamePrefix));
                if (!string.IsNullOrWhiteSpace(optimizedItemLink2.Url))
                {
                    AddOrMerge(result, optimizedItemLink2);
                }
            }
        }

        protected void AddOrMerge(SpeedyAssetLinks result, SpeedyAssetLink optimizedItemLink2)
        {
            var scriptName = GetScriptName(optimizedItemLink2.Url);
            if (!result.OrderedScripts.ContainsKey(optimizedItemLink2.LoadingOrder))
            {
                var speedyDictionary = new Dictionary<string, SpeedyAssetLink>();
                speedyDictionary.Add(scriptName, optimizedItemLink2);
                result.OrderedScripts.Add(optimizedItemLink2.LoadingOrder, speedyDictionary);
            }
            else
            {
                var matchingPhase = result.OrderedScripts[optimizedItemLink2.LoadingOrder];
                if (!matchingPhase.ContainsKey(scriptName))
                    matchingPhase.Add(scriptName, optimizedItemLink2);
            }
        }

        protected virtual SpeedyAssetLink GetOptimizedItemLinkSpeedy(Item theme, OptimizationType type, AssetServiceMode mode, string query, string fileName)
        {
            query = string.Format(query, Templates.OptimizedFile.ID, fileName);
            Item item = theme.Axes.SelectSingleItem(query);
            var loadOrderInt = 100;
            var path = string.Empty;
            if (item != null && IsNotEmpty(item))
            {
                path = item.BuildAssetPath(addTimestamp: true);
                var loadOrder = item.Fields[SpeedyConstants.SpeedyOptimizedFileTemplate.Fields.LoadOrder].Value;
               
                if (string.IsNullOrWhiteSpace(loadOrder))
                    Int32.TryParse(loadOrder, out loadOrderInt);

                return new SpeedyAssetLink() { LoadingOrder = loadOrderInt, Url = path };
            }

            path = new AssetBundler().GetOptimizedItemPath(theme, type, mode);
            return new SpeedyAssetLink() { LoadingOrder = loadOrderInt, Url = path };
        }

        public bool IsNotEmpty(Item optimizedScriptItem)
        {
            using (Stream stream = ((MediaItem)optimizedScriptItem).GetMediaStream())
            {
                return stream != null && stream.Length > 0;
            }
        }
    }
}