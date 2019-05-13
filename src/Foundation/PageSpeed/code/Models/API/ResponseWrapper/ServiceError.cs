namespace Site.Foundation.PageSpeed.Models.API.ResponseWrapper
{
    public class ServiceError : IServiceError
    {
        public ServiceError()
        {
        }

        public ServiceError(string message, string errorCode)
        {
            Message = message;
            ErrorCode = errorCode;
        }

        public string ErrorCode { get; set; }
        public string Message { get; set; }
    }
}