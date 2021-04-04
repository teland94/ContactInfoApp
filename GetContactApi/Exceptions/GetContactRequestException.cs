using GetContactAPI.Models;
using System.Net;
using System.Net.Http;

namespace GetContactAPI.Exceptions
{
    public class GetContactRequestException : HttpRequestException
    {
        public HttpStatusCode StatusCode { get; set; }

        public ApiResponse<ErrorResult> ErrorInfo { get; set; }

        public GetContactRequestException(string message, HttpStatusCode statusCode, ApiResponse<ErrorResult> errorInfo) : base(message)
        {
            StatusCode = statusCode;
            ErrorInfo = errorInfo;
        }
    }
}
