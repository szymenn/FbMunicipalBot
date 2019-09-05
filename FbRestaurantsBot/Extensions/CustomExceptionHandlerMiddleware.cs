using System;
using System.Threading.Tasks;
using FbRestaurantsBot.Exceptions;
using FbRestaurantsBot.Helpers;
using FbRestaurantsBot.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FbRestaurantsBot.Extensions
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private IMessengerClient _messengerClient;

        public CustomExceptionHandlerMiddleware(RequestDelegate next, IMessengerClient messengerClient)
        {
            _next = next;
            _messengerClient = messengerClient;
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