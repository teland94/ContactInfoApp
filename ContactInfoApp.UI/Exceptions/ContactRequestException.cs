using System;
using System.Net;
using System.Text.Json.Serialization;

namespace ContactInfoApp.UI.Exceptions
{
    public class ContactRequestException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public ErrorResult ErrorResult { get; }

        public ContactRequestException(HttpStatusCode statusCode, string message, ErrorResult errorResult = null): base(message)
        {
            StatusCode = statusCode;
            ErrorResult = errorResult;
        }
    }

    public class ErrorResult
    {
        [JsonPropertyName("image")]
        public string Image { get; set; }
    }
}
