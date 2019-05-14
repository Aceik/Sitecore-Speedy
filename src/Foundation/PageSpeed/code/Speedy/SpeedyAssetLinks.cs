using System.Collections.Generic;
using Sitecore.XA.Foundation.Theming.Bundler;

namespace Site.Foundation.PageSpeed.Speedy
{
    public class SpeedyAssetLinks : AssetLinks
    {
        public SpeedyAssetLinks(AssetLinks links)
        {
            this.Scripts = links.Scripts;
            this.Styles = links.Styles;
        }
        public SpeedyAssetLinks()
        {
            OrderedScripts = new Dictionary<int, Dictionary<string, SpeedyAssetLink>>();
        }

        public Dictionary<int, Dictionary<string, SpeedyAssetLink>> OrderedScripts { get; set; }

        public string ClientScriptsRendered { get; set; }
    }
}