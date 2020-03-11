using Newtonsoft.Json;
using System.Collections.Generic;

namespace Sitecore.Foundation.Speedy.Model
{
    public class RemoteCriticalParameters
    {     
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "width")]
        public string Width { get; set; }

        [JsonProperty(PropertyName = "height")]
        public string Height { get; set; }

        [JsonProperty(PropertyName = "fontMap")]
        public List<FontMap> FontMap { get; set; }

        [JsonProperty(PropertyName = "removeDuplicates")]
        public List<FindDuplicatesMap> RemoveDuplicates { get; set; }

        [JsonProperty(PropertyName = "fontFaceSwitch")]
        public List<FontMap> FontFaceSwitch { get; set; }
    }
}