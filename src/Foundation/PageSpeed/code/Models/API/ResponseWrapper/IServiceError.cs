namespace Site.Foundation.PageSpeed.Models.API.ResponseWrapper
{
    public interface IServiceError
    {
        string ErrorCode { get; set; }

        string Message { get; set; }
    }
}