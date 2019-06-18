using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace Site.Foundation.Speedy.Models.API.ResponseWrapper
{
    public class WebApiResponse<T> : IWebApiResponse<T>
    {
        public WebApiResponse(T data)
        {
            Success = true;
            Errors = null;
            Data = data;
        }

        public bool Success { get; set; }

        public IList<IServiceError> Errors { get; set; }

        public T Data { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}