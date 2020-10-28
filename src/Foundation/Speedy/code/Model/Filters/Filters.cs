using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Sitecore.Foundation.Speedy.Model.Filters
{
    public class Filters
    {
        [JsonProperty(PropertyName = "filters")]
        public List<RemoveFilter> FilterList { get; set; }
    }

    public class RemoveFilter
    {
        [JsonProperty(PropertyName = "start")]
        public string Start { get; set; }
        [JsonProperty(PropertyName = "end")]
        public string End { get; set; }
    }
}