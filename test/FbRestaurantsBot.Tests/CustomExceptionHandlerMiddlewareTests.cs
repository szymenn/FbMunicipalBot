using System;
using System.IO;
using System.Threading.Tasks;
using FbRestaurantsBot.Exceptions;
using FbRestaurantsBot.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace FbRestaurantsBot.Tests
{
    public class CustomExceptionHandlerMiddlewareTests
    {
        private readonly HttpContext _httpContext;

        public CustomExceptionHandlerMiddlewareTests()
        {
            _httpContext = new DefaultHttpContext();
        }
        
        [Fact]
        public async Task Invoke_WhenVerificationException_Sets403StatusCode()
        {
            var middleware = new CustomExceptionHandlerMiddleware
                (context => throw new VerificationException(It.IsAny<string>()));

            await middleware.Invoke(_httpContext);
            
            Assert.Equal(StatusCodes.Status403Forbidden, _httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task Invoke_WhenApiCallException_SetsCorrectStatusCode()
        {
            var middleware = new CustomExceptionHandlerMiddleware
            (context => throw new ApiCallException(It.IsAny<string>(), 
                StatusCodes.Status400BadRequest,
                It.IsAny<string>()));

            await middleware.Invoke(_httpContext);
            
            Assert.Equal(StatusCodes.Status400BadRequest, _httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task Invoke_WhenMessengerException_Sets400StatusCode()
        {
            var middleware = new CustomExceptionHandlerMiddleware
                (context => throw new MessengerException(It.IsAny<string>()));

            await middleware.Invoke(_httpContext);
            
            Assert.Equal(StatusCodes.Status400BadRequest, _httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task Invoke_WhenUnknownException_Sets500StatusCode()
        {
            var middleware = new CustomExceptionHandlerMiddleware
                (context => throw new Exception());

            await middleware.Invoke(_httpContext);
            
            Assert.Equal(StatusCodes.Status500InternalServerError, _httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task Invoke_WhenVerificationException_WritesProblemDetails()
        {
            var middleware = new CustomExceptionHandlerMiddleware
                (context => throw new System.Security.VerificationException(It.IsAny<string>()));
            
            _httpContext.Response.Body = new MemoryStream();

            await middleware.Invoke(_httpContext);
            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            
            var jsonString = new StreamReader(_httpContext.Response.Body).ReadToEnd();
            var result = JsonConvert.DeserializeObject<ProblemDetails>(jsonString);

            Assert.IsType<ProblemDetails>(result);
        }


        [Fact]
        public async Task Invoke_WhenApiCallException_WritesProblemDetails()
        {
            var middleware = new CustomExceptionHandlerMiddleware
                (context => throw new ApiCallException
                (It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()));
             
            _httpContext.Response.Body = new MemoryStream();

            await middleware.Invoke(_httpContext);
            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            
            var jsonString = new StreamReader(_httpContext.Response.Body).ReadToEnd();
            var result = JsonConvert.DeserializeObject<ProblemDetails>(jsonString);

            Assert.IsType<ProblemDetails>(result);
        }

        [Fact]
        public async Task Invoke_WhenMessengerException_WritesProblemDetails()
        {
            var middleware = new CustomExceptionHandlerMiddleware
                (context => throw new MessengerException(It.IsAny<string>()));
            
            _httpContext.Response.Body = new MemoryStream();

            await middleware.Invoke(_httpContext);
            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            
            var jsonString = new StreamReader(_httpContext.Response.Body).ReadToEnd();
            var result = JsonConvert.DeserializeObject<ProblemDetails>(jsonString);

            Assert.IsType<ProblemDetails>(result);
        }

        [Fact]
        public async Task Invoke_WhenUnknownException_WritesProblemDetails()
        {
            var middleware = new CustomExceptionHandlerMiddleware
                (context => throw new Exception());
            
            _httpContext.Response.Body = new MemoryStream();

            await middleware.Invoke(_httpContext);
            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            
            var jsonString = new StreamReader(_httpContext.Response.Body).ReadToEnd();
            var result = JsonConvert.DeserializeObject<ProblemDetails>(jsonString);

            Assert.IsType<ProblemDetails>(result);
        }
    }
}