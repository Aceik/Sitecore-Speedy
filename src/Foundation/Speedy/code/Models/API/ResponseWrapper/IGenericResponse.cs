namespace Site.Foundation.Speedy.Models.API.ResponseWrapper
{
    public interface IWebApiResponse<T>
    {
        T Data { get; set; }
    }
}