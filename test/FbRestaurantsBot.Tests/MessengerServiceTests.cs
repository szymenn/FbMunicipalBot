using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FbRestaurantsBot.Core.Configuration;
using FbRestaurantsBot.Core.Dtos.Messaging;
using FbRestaurantsBot.Core.Dtos.Restaurants;
using FbRestaurantsBot.Core.Exceptions;
using FbRestaurantsBot.Core.Interfaces;
using FbRestaurantsBot.Core.Services;
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
        private readonly Mock<IZomatoClient> _zomatoClientMock;
        private readonly MessengerService _messengerService;
        private const string Token = "token";
        private const string Mode = "mode";
        
        public MessengerServiceTests()
        {
            _loggerMock = new Mock<ILoggerAdapter>();
            _messengerClientMock = new Mock<IMessengerClient>();
            _zomatoClientMock = new Mock<IZomatoClient>();
            
            var fbOptions = Options.Create(new FacebookSettings
            {
                VerifyToken = Token,
                Mode = Mode,
                Object = "page"
            });
            
            _messengerService =  new MessengerService
                (fbOptions, _messengerClientMock.Object, _zomatoClientMock.Object, _loggerMock.Object);

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
        public async Task Receive_WhenAttachmentsMessage_CallsMessengerClient()
        {
            _zomatoClientMock.Setup(e => e.CallZomatoApi
                    (It.IsAny<double>(), It.IsAny<double>()))
                .Returns(Task.FromResult(CreateNearby()));

            var httpRequestStub = CreateHttpRequestStub(GetAttachmentsMessageRequestJson());

            await _messengerService.Receive(httpRequestStub.Object);

            _messengerClientMock.Verify(e => e.CallSendApi
                (It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Receive_WhenTextMessage_CallsMessengerClient()
        {
            var httpRequestStub = CreateHttpRequestStub(GetTextMessageRequestJson());

            await _messengerService.Receive(httpRequestStub.Object);

            _messengerClientMock.Verify
                (e => e.CallSendApi
                (It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Receive_WhenObjectNotValid_ThrowsMessengerException()
        {
            var httpRequestStub = CreateHttpRequestStub(GetInvalidObjectRequestJson());

            await Assert.ThrowsAsync<MessengerException>
                (() => _messengerService.Receive(httpRequestStub.Object));
        }
        
        

        private static Mock<HttpRequest> CreateHttpRequestStub(string json)
        {
            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream);

            streamWriter.Write(json);
            streamWriter.Flush();

            memoryStream.Position = 0;

            var httpRequestStub = new Mock<HttpRequest>();
            httpRequestStub.Setup(e => e.Body).Returns(memoryStream);
            return httpRequestStub;
        }

        private static string GetAttachmentsMessageRequestJson()
        {
            return JsonConvert.SerializeObject(new WebHookRequest
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
                                Message = new Message
                                {
                                    Attachments = new List<Attachment>
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
        }

        private static string GetTextMessageRequestJson()
        {
            return JsonConvert.SerializeObject(new WebHookRequest
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
                                Message = new Message
                                {
                                    Attachments = null,
                                    Text = "message"
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
        }

        private static string GetInvalidObjectRequestJson()
        {
            return JsonConvert.SerializeObject(new WebHookRequest
            {
                Object = It.Is<string>(m => !m.Equals("page")),
                Entry = It.IsAny<ICollection<Entry>>()
            });
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