namespace Site.Foundation.PageSpeed.Models.API.ResponseWrapper
{
    public interface IWebApiResponse<T> : IWebApiResponse
    {
        T Data { get; set; }
    }
}