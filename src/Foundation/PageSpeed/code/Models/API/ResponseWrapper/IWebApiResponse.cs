using System.Collections.Generic;

namespace Site.Foundation.PageSpeed.Models.API.ResponseWrapper
{
    public interface IWebApiResponse
    {
        bool Success { get; set; }

        IList<IServiceError> Errors { get; set; }
    }
}