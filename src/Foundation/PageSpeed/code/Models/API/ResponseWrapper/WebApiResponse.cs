using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace Site.Foundation.PageSpeed.Models.API.ResponseWrapper
{
    public class WebApiResponse : IWebApiResponse
    {
        public WebApiResponse()
        {
            Success = true;
        }

        public WebApiResponse(string errorMessage, string code)
        {
            Errors = new List<IServiceError>
            {
                new ServiceError(errorMessage, code)
            };
            Success = false;
        }

        public WebApiResponse(IList<IServiceError> ServiceErrors)
        {
            Errors = ServiceErrors;
        }

        public WebApiResponse(Exception ex)
        {
            Errors = ReturnExceptionsList(ex);
        }

        public WebApiResponse(HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage.IsSuccessStatusCode) Success = true;
            if (httpResponseMessage.Content != null)
            {
                var response =
                    JsonConvert.DeserializeObject<WebApiResponse>(
                        httpResponseMessage.Content.ReadAsStringAsync().Result);
                Errors = response.Errors;
                Success = response.Success;
            }
        }

        public bool Success { get; set; }

        public IList<IServiceError> Errors { get; set; }

        public static IList<IServiceError> ReturnExceptionsList(Exception ex)
        {
            var errors = new List<ServiceError>();

            var aggregateException = ex as AggregateException;
            if (aggregateException != null)
                foreach (var inner in aggregateException.InnerExceptions)
                    errors.AddRange((IList<ServiceError>) ReturnExceptionsList(inner));
            else if (ex is TimeoutException)
                errors.Add(new ServiceError("Timeout", "T01"));
            else
                errors.Add(new ServiceError("RuntimeError", "RE01"));

            return (IList<IServiceError>) errors;
        }
    }

    public class WebApiResponse<T> : IWebApiResponse<T>
    {
        public WebApiResponse()
        {
        }


        public WebApiResponse(List<IServiceError> serviceErrors)
        {
            Errors = serviceErrors;
        }

        public WebApiResponse(string errorMessage, string errorCode)
        {
            var result = new List<ServiceError>();
            result.Add(new ServiceError(errorMessage, errorCode));
            Errors = result.ToList<IServiceError>();
            Success = false;
        }

        public WebApiResponse(ServiceError serviceError)
        {
            var result = new List<ServiceError>();
            result.Add(serviceError);
            Errors = result.ToList<IServiceError>();
            Success = false;
        }

        public WebApiResponse(Exception ex)
        {
            Errors = WebApiResponse.ReturnExceptionsList(ex).ToList();
            Success = false;
        }

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