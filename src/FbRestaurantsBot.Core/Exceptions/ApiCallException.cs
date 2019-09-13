using System;

namespace FbRestaurantsBot.Core.Exceptions
{
    public class ApiCallException : Exception
    {
        public int StatusCode { get; }
        public string ReasonPhrase { get; }

        public ApiCallException(string message)
            : base(message)
        {
            
        }

        public ApiCallException(string message, Exception inner)
            : base(message, inner)
        {
            
        }
        
        public ApiCallException(string message, int statusCode, string reasonPhrase)
            : base(message)
        {
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
        }

        public ApiCallException(string message, int statusCode, string reasonPhrase, Exception inner)
            : base(message, inner)
        {
            
        }

    }
}