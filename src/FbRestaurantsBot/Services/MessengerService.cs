using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FbRestaurantsBot.Configuration;
using FbRestaurantsBot.Exceptions;
using FbRestaurantsBot.Models.Messaging;
using FbRestaurantsBot.Models.Restaurants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VerificationException = System.Security.VerificationException;

namespace FbRestaurantsBot.Services
{
    public class MessengerService : IMessengerService
    {
        private readonly FacebookSettings _fbSettings;
        private readonly IMessengerClient _messengerClient;
        private readonly ILogger<MessengerService> _logger;
        private IZomatoApiClient _zomatoClient;

        public MessengerService(IOptions<FacebookSettings> fbSettings, 
            IMessengerClient messengerClient,
            IZomatoApiClient zomatoClient,
            ILogger<MessengerService> logger)
        {
            _fbSettings = fbSettings.Value;
            _messengerClient = messengerClient;
            _zomatoClient = zomatoClient;
            _logger = logger;
        }

        public async Task Receive(HttpRequest request)
        {
            var webHookRequest = ParseRequestBody(request);
            if (webHookRequest.Object == _fbSettings.Object)
            {
                await CallSendApi(webHookRequest);
            }
            else
            {
                throw new MessengerException("Object is not equal to page");
            }
        }

        public void VerifyToken(string token, string mode)
        {
            if (mode != null && token != null)
            {
                if (token == _fbSettings.VerifyToken && mode == _fbSettings.Mode)
                {
                    _logger.LogInformation("WEBHOOK VERIFIED");
                }
            }
            else
            {
                throw new VerificationException("Unable to verify");
            }
        }

        private WebHookRequest ParseRequestBody(HttpRequest request)
        {
            using (var reader = new StreamReader(request.Body))
            {
                var body = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<WebHookRequest>(body);
            }
        }

        private async Task CallSendApi(WebHookRequest webHookRequest)
        {
            foreach (var entry in webHookRequest.Entry)
            {
                var webHookEvent = entry.Messaging[0];
                if (webHookEvent.Message != null && webHookEvent.Sender != null)
                {
                    if (webHookEvent.Message.Text != null)
                    {
                        await _messengerClient.CallSendApi(webHookEvent.Sender.Id, "hehe odpowiadam");
                    }
                    else if (webHookEvent.Message.Attachments != null)
                    {
                        var attachment = webHookEvent.Message.Attachments[0];
                        if (attachment.Type == "location")
                        {
                            var nearby= await _zomatoClient.CallZomatoApi
                            (attachment.Payload.Coordinates.Latitude,
                                attachment.Payload.Coordinates.Longitude);
                            _logger.LogInformation("OK");


                            await _messengerClient.CallSendApi(webHookEvent.Sender.Id,
                                GetRestaurantsString(nearby.Restaurants));
                        }
                    }
                }
                if (webHookEvent.Postback != null)
                {
                    throw new NotImplementedException();
                }
                
            }
        }

        private string GetRestaurantsString(ICollection<RestaurantWrapper> restaurants)
        {
            return restaurants.Aggregate("",
                (current, next) => current + (next.Restaurant.Name + "\n" + next.Restaurant.Location.Address +
                                              "\n" + next.Restaurant.Url + "\n\n"));
        }
        

        private async Task<Nearby> CallZomatoApi(Attachment attachment)
        {
            return await _zomatoClient.CallZomatoApi
            (attachment.Payload.Coordinates.Latitude,
                attachment.Payload.Coordinates.Longitude);
        }
        
    }
}