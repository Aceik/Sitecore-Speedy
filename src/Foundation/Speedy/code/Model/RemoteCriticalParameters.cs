using Newtonsoft.Json;
using System.Collections.Generic;

namespace Sitecore.Foundation.Speedy.Model
{
    public class RemoteCriticalParameters
    {     
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "width")]
        public int Width { get; set; }

        [JsonProperty(PropertyName = "height")]
        public int Height { get; set; }

        [JsonProperty(PropertyName = "fontmap")]
        public List<FontMap> FontMap { get; set; }
    }
}