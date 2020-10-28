using System.Collections.Generic;
using Sitecore.XA.Foundation.Theming.Bundler;

namespace Sitecore.Foundation.Speedy.Model
{
    public class SpeedyLayoutModel
    {
        public bool SpeedyEnabled { get; set; }
        public bool SpeedyJsEnabled { get; set; }
        public bool SpeedyCssEnabled { get; set; }
        public string VanillaJavasript { get; set; }
        public string VanillaJavasriptAllLoads { get; set; }
        public bool ByPassNotDetected { get; set; }
        public AssetLinks AssetLinks { get; set; }
        public string CriticalHtml { get; set; }
        public string SpecialCaseCriticalCss { get; set; }
        public string CacheKey { get; set; }
    }
}