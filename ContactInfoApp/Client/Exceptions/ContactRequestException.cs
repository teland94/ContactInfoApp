using System;
using System.Net;
using ContactInfoApp.Shared;

namespace ContactInfoApp.Client.Exceptions
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
}
