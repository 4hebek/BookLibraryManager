using System.Net;

namespace LibraryManager.Core.Contracts
{
    public class Result<TSuccess, TError>
    {
        public TSuccess Success { get; private set; }
        public TError Error { get; private set; }
        public bool IsSuccess { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }

        private Result()
        {
            IsSuccess = false;
        }

        public static Result<TSuccess, TError> SuccessResponse(TSuccess success, HttpStatusCode statusCode)
        {
            return new Result<TSuccess, TError> { Success = success, IsSuccess = true, StatusCode = statusCode };
        }

        public static Result<TSuccess, TError> ErrorResponse(TError error, HttpStatusCode statusCode)
        {
            return new Result<TSuccess, TError> { Error = error, StatusCode = statusCode };
        }
    }
}