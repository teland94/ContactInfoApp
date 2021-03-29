using System.Net;
using System.Net.Http;

namespace GetContactAPI.Exceptions
{
    public class GetContactRequestException : HttpRequestException
    {
        public HttpStatusCode StatusCode { get; set; }

        public GetContactRequestException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
