using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Site.Foundation.PageSpeed.Models.API.ResponseWrapper
{
    public interface IApiErrorResponse
    {
        [DataMember]
        [JsonProperty(PropertyName = "ErrorType")]
        string ErrorType { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "ErrorMessage")]
        string ErrorMessage { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "Data")]
        string Data { get; set; }
    }

    public class ApiErrorResponse : IApiErrorResponse
    {
        public string ErrorType { get; set; }

        public string ErrorMessage { get; set; }

        public string Data { get; set; }
    }
}