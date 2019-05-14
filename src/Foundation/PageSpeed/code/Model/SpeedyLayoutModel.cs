using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.XA.Foundation.Theming.Bundler;

namespace Site.Foundation.PageSpeed.Model
{
    public class SpeedyLayoutModel
    {
        public bool SpeedyEnabled { get; set; }
        public bool SpeedyJsEnabled { get; set; }
        public bool SpeedyCssEnabled { get; set; }
        public bool ByPassNotDetected { get; set; }
        public AssetLinks AssetLinks { get; set; }
    }
}