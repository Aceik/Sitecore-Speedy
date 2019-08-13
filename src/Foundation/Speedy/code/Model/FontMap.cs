using Newtonsoft.Json;

namespace Sitecore.Foundation.Speedy.Model
{
    public class FontMap
    {
        [JsonProperty(PropertyName = "find")]
        public string Find { get; set; }
        [JsonProperty(PropertyName = "replace")]
        public string Replace { get; set; }
    }
}