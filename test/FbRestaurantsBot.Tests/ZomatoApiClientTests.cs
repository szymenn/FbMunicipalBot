using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using FbRestaurantsBot.Configuration;
using FbRestaurantsBot.Exceptions;
using FbRestaurantsBot.Models.Restaurants;
using FbRestaurantsBot.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Xunit;

namespace FbRestaurantsBot.Tests
{
    public class ZomatoApiClientTests
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<ZomatoSettings> _zomatoOptions;
        private readonly HttpResponseMessage _response;
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private const string BaseUrl = "https://hehe.com/";
        public ZomatoApiClientTests()
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
            
            _httpClient =  new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri(BaseUrl)
            };

            _zomatoOptions = Options.Create(new ZomatoSettings
            {
                ApiKey = It.IsAny<string>()
            });
            
        }
        
        [Fact]
        public async Task CallZomatoApi_ByDefault_ReturnsNearby()
        {
            var zomatoApiClient = new ZomatoApiClient(_httpClient, _zomatoOptions);
            var result = await zomatoApiClient.CallZomatoApi(It.IsAny<double>(), It.IsAny<double>());

            Assert.IsType<Nearby>(result);
        }

        [Fact]
        public async Task CallZomatoApi_ByDefault_HttpClientGet()
        {
            var zomatoApiClient = new ZomatoApiClient(_httpClient, _zomatoOptions);
            var result = await zomatoApiClient.CallZomatoApi(It.IsAny<double>(), It.IsAny<double>());

            _handlerMock.Protected().Verify("SendAsync", Times.Once(),
                ItExpr.Is<HttpRequestMessage>
                    (req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task CallZomatoApi_ByDefault_CallsRightUri()
        {
            var zomatoApiClient = new ZomatoApiClient
                (_httpClient, _zomatoOptions);
            var result = await zomatoApiClient.CallZomatoApi(It.IsAny<double>(), It.IsAny<double>());
            
            var expectedUri = new Uri(BaseUrl + "geocode?lat=0.0000&lon=0.0000");
            
            _handlerMock.Protected().Verify("SendAsync", Times.Once(),
                ItExpr.Is<HttpRequestMessage>
                    (req => req.RequestUri == expectedUri),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task CallZomatoApi_WhenRequestNotSuccessful_ThrowsApiCallException()
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            
            var zomatoApiClient = new ZomatoApiClient(_httpClient, _zomatoOptions);

            await Assert.ThrowsAsync<ApiCallException>
                (() => zomatoApiClient.CallZomatoApi(It.IsAny<double>(), It.IsAny<double>()));
        }
    }
}