using System;
using System.Threading.Tasks;
using FbRestaurantsBot.Core.Exceptions;
using FbRestaurantsBot.Core.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FbRestaurantsBot.Api.Extensions
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var problemDetails = new ProblemDetails
            {
                Instance = $"urn:messengerbot:{Guid.NewGuid()}"
            };

            try
            {
                await _next(context);
            }
            catch (VerificationException verificationException)
            {
                problemDetails.Status = verificationException.StatusCode;
                problemDetails.Detail = verificationException.Message;
                problemDetails.Title = verificationException.ReasonPhrase;
                context.Response.StatusCode = verificationException.StatusCode;
                context.Response.WriteJson(problemDetails);
            }
            catch (ApiCallException apiCallException)
            {
                problemDetails.Status = apiCallException.StatusCode;
                problemDetails.Detail = apiCallException.Message;
                problemDetails.Title = apiCallException.ReasonPhrase;
                context.Response.StatusCode = apiCallException.StatusCode;
                context.Response.WriteJson(problemDetails);
            }
            catch (MessengerException messengerException)
            {
                problemDetails.Status = messengerException.StatusCode;
                problemDetails.Detail = messengerException.Message;
                problemDetails.Title = messengerException.ReasonPhrase;
                context.Response.StatusCode = messengerException.StatusCode;
                context.Response.WriteJson(problemDetails);
            }
            catch (Exception exception)
            {
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Detail = exception.Message;
                problemDetails.Title = Constants.InternalServerError;
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.WriteJson(problemDetails);
            }
        }
    }
}