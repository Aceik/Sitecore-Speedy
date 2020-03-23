using Newtonsoft.Json;

namespace Sitecore.Foundation.ImageCompression.Model
{
    public class ImageUpload
    {
        [JsonProperty(PropertyName = "input")]
        public Input InputObj { get; set; }
        public string Location { get; set; }

        public class Input
        {
            [JsonProperty(PropertyName = "size")]
            public long Size { get; set; }
            [JsonProperty(PropertyName = "type")]
            public string Type { get; set; }
        }
    }
}