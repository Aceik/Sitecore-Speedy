using Sitecore.XA.Foundation.Theming.Bundler;

namespace Site.Foundation.PageSpeed.Speedy
{
    public class SpeedyAssetLink : AssetLinks
    {
        public string Url { get; set; }
        public int LoadingOrder { get; set; }
    }
}