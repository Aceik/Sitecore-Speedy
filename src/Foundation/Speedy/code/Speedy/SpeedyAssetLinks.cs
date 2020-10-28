using System.Collections.Generic;
using Sitecore.XA.Foundation.Theming.Bundler;

namespace Sitecore.Foundation.Speedy.Speedy
{
    public class SpeedyAssetLinks : AssetLinks
    {
        public SpeedyAssetLinks(SpeedyAssetLinks links)
        {
            this.Scripts = links.Scripts;
            this.Styles = links.Styles;
            PlainStyles = links.PlainStyles;
            PrefetchStyles = links.PrefetchStyles;
        }
        public SpeedyAssetLinks()
        {
            OrderedScripts = new Dictionary<int, Dictionary<string, AssetLinks>>();
            PlainStyles = new HashSet<string>();
            PrefetchStyles = new HashSet<string>();
        }

        public HashSet<string> PlainStyles { get; set; }
        public HashSet<string> PrefetchStyles { get; set; }
        public Dictionary<int, Dictionary<string, AssetLinks>> OrderedScripts { get; set; }

        public string ClientScriptsRendered { get; set; }
        public string ClientScriptsPreload { get; set; }
    }
}