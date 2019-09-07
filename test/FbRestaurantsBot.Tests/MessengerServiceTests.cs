using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FbRestaurantsBot.Configuration;
using FbRestaurantsBot.Exceptions;
using FbRestaurantsBot.Models.Messaging;
using FbRestaurantsBot.Models.Restaurants;
using FbRestaurantsBot.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace FbRestaurantsBot.Tests
{
    public class MessengerServiceTests
    {
        private readonly Mock<ILoggerAdapter> _loggerMock;
        private readonly Mock<IMessengerClient> _messengerClientMock;
        private readonly Mock<IZomatoApiClient> _zomatoApiClientMock;
        private readonly MessengerService _messengerService;
        private const string Token = "token";
        private const string Mode = "mode";
        
        public MessengerServiceTests()
        {
            _loggerMock = new Mock<ILoggerAdapter>();
            _messengerClientMock = new Mock<IMessengerClient>();
            _zomatoApiClientMock = new Mock<IZomatoApiClient>();
            
            var fbOptions = Options.Create(new FacebookSettings
            {
                VerifyToken = Token,
                Mode = Mode,
                Object = "page"
            });
            
            _messengerService =  new MessengerService
                (fbOptions, _messengerClientMock.Object, _zomatoApiClientMock.Object, _loggerMock.Object);

        }
        
        [Fact]
        public void VerifyToken_ByDefault_CallsLogger()
        {
            _messengerService.VerifyToken(Token, Mode);
            
            _loggerMock.Verify(e => e.LogInformation(It.IsAny<string>()), Times.Once);
        }
        
        [Fact]
        public void VerifyToken_WhenInvalidTokenOrMode_ThrowsVerificationException()
        {
            var invalidToken = It.Is<string>(m => !m.Equals(Token));
            var invalidMode = It.Is<string>(m => !m.Equals(Mode));

            Assert.Throws<VerificationException>
                (() => _messengerService.VerifyToken(invalidToken, invalidMode));
        }

        [Fact]
        public async Task Receive_ByDefault_CallsMessengerService()
        {
            _zomatoApiClientMock.Setup(e => e.CallZomatoApi
                    (It.IsAny<double>(), It.IsAny<double>()))
                .Returns(Task.FromResult(CreateNearby()));


            var httpRequestStub = CreateHttpRequestStub();

            await _messengerService.Receive(httpRequestStub.Object);

            _messengerClientMock.Verify(e => e.CallSendApi
                (It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        private static Mock<HttpRequest> CreateHttpRequestStub()
        {
            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream);

            var json = JsonConvert.SerializeObject(new WebHookRequest
            {
                Object = "page",
                Entry = new List<Entry>
                {
                    new Entry
                    {
                        Messaging = new List<Messaging>
                        {
                            new Messaging
                            {
                                Message = new Message{Attachments = new List<Attachment>
                                    {
                                        new Attachment
                                        {
                                            Type = "location",
                                            Payload = new Payload
                                            {
                                                Coordinates = new Coordinates
                                                {
                                                    Latitude = It.IsAny<double>(),
                                                    Longitude = It.IsAny<double>()
                                                }
                                            }
                                        }
                                    },
                                    Text = null
                                },
                                Sender = new Sender()
                                {
                                    Id = "some id"
                                }
                            }
                        }
                    }
                }
            });
            
            streamWriter.Write(json);
            streamWriter.Flush();

            memoryStream.Position = 0;

            var httpRequestStub = new Mock<HttpRequest>();
            httpRequestStub.Setup(e => e.Body).Returns(memoryStream);
            return httpRequestStub;
        }

        private static Nearby CreateNearby()
        {
            return new Nearby
            {    
                Restaurants = new List<RestaurantWrapper>
                {
                    new RestaurantWrapper
                    {
                        Restaurant = new Restaurant
                        {
                            Location = new Location
                            {
                                Address = It.IsAny<string>(),
                                City = It.IsAny<string>(),
                                Locality = It.IsAny<string>()
                            },
                            Name = It.IsAny<string>(),
                            Url = It.IsAny<string>()
                        }
                    }
                }
            };
        }
    }
}