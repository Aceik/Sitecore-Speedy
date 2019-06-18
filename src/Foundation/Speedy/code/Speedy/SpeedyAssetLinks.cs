using System.Collections.Generic;
using Sitecore.XA.Foundation.Theming.Bundler;

namespace Sitecore.Foundation.Speedy.Speedy
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
            OrderedScripts = new Dictionary<int, Dictionary<string, AssetLinks>>();
        }

        public Dictionary<int, Dictionary<string, AssetLinks>> OrderedScripts { get; set; }

        public string ClientScriptsRendered { get; set; }
    }
}