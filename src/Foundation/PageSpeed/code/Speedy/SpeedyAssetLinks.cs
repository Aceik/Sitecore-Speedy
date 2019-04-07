using System.Collections.Generic;
using Sitecore.XA.Foundation.Theming.Bundler;

namespace Site.Foundation.PageSpeed.Speedy
{
    public class SpeedyAssetLinks : AssetLinks
    {
        public SpeedyAssetLinks()
        {
            OrderedScripts = new Dictionary<int, Dictionary<string, SpeedyAssetLink>>();
        }

        //public HashSet<SpeedyAssetLink> OrderedScripts { get; set; }
                
        //public Dictionary<string, string> ClientScriptsDictionary { get; set; }
        //public Dictionary<string, string> TimestampDictionary { get; set; }
        
        //public string CoreLibrariesScripts { get; set; }
        public Dictionary<int, Dictionary<string, SpeedyAssetLink>> OrderedScripts { get; set; }

        public string ClientScriptsRendered { get; set; }
    }
}