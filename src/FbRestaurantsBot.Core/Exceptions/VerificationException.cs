using System;
using FbRestaurantsBot.Core.Helpers;
using Microsoft.AspNetCore.Http;

namespace FbRestaurantsBot.Core.Exceptions
{
    public class VerificationException : Exception
    {
        public int StatusCode { get; }
        public string ReasonPhrase { get; }

        public VerificationException()
        {
            StatusCode = StatusCodes.Status403Forbidden;
            ReasonPhrase = Constants.Forbidden;
        }

        public VerificationException(string message)
            : base(message)
        {
            StatusCode = StatusCodes.Status403Forbidden;
            ReasonPhrase = Constants.Forbidden;

        }

        public VerificationException(string message, Exception inner)
            : base(message, inner)
        {
            StatusCode = StatusCodes.Status403Forbidden;
            ReasonPhrase = Constants.Forbidden;
        }
    }
}