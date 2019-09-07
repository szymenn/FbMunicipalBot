using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using FbRestaurantsBot.Configuration;
using FbRestaurantsBot.Exceptions;
using FbRestaurantsBot.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Xunit;

namespace FbRestaurantsBot.Tests
{
    public class MessengerClientTests
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<FacebookSettings> _fbOptions;
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private const string BaseUrl = "https://hehe.com/";
        private readonly HttpResponseMessage _response;

        public MessengerClientTests()
        {
            _response = new HttpResponseMessage
            {
                Content = new StringContent("{'location':{'nearby_restaurants':[]}}'")
            };
            _response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            
            _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>
                ("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(_response)
                .Verifiable();


            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri(BaseUrl)
            };
            
            _fbOptions = Options.Create(new FacebookSettings
            {
                Secret = It.IsAny<string>()
            });
        }

        [Fact]
        public async Task CallSendApi_ByDefault_CallsHttpClientPost()
        {
            var messengerClient = new MessengerClient
                (_httpClient, _fbOptions);
            
            await messengerClient.CallSendApi(It.IsAny<string>(), It.IsAny<string>());
            
            _handlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>
                (req => req.Method == HttpMethod.Post), ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task CallSendApi_ByDefault_CallsRightUri()
        {
            var messengerClient = new MessengerClient
                (_httpClient, _fbOptions);

            await messengerClient.CallSendApi(It.IsAny<string>(), It.IsAny<string>());
            var expectedUri = new Uri(BaseUrl + $"?access_token=");
            
            _handlerMock.Protected().Verify("SendAsync", Times.Once(),
                ItExpr.Is<HttpRequestMessage>
                    (req => req.RequestUri == expectedUri),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task CallSendApi_WhenRequestNotSuccessful_ThrowsApiCallException()
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            
            var messengerClient = new MessengerClient
                (_httpClient, _fbOptions);

            await Assert.ThrowsAsync<ApiCallException>
                (() => messengerClient.CallSendApi
                (It.IsAny<string>(), It.IsAny<string>()));
        }
    }
}