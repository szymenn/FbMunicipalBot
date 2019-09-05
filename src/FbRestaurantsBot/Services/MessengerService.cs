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
        private readonly IZomatoApiClient _zomatoClient;

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
                await HandleWebHookRequest(webHookRequest);
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

        private async Task HandleWebHookRequest(WebHookRequest webHookRequest)
        {
            foreach (var entry in webHookRequest.Entry)
            {
                await HandleWebHookEvents(entry.Messaging);
            }
        }


        private async Task HandleWebHookEvents(IEnumerable<Messaging> webHookEvents)
        {
            foreach (var webHookEvent in webHookEvents)
            {
                await HandleWebHookEvent(webHookEvent);
            }
        }

        private async Task HandleWebHookEvent(Messaging webHookEvent)
        {
            if (webHookEvent.Message != null && webHookEvent.Sender != null)
            {
                await HandleTextMessage(webHookEvent.Message.Text, webHookEvent.Sender.Id);
                await HandleAttachmentsMessage(webHookEvent.Message.Attachments, webHookEvent.Sender.Id);
            }
            if (webHookEvent.Postback != null)
            {
                throw new NotImplementedException();
            }
        }
        
        private async Task HandleTextMessage(string text, string senderId)
        {
            if (text != null)
            {
                await SendBasicResponse(senderId);
            }
        }

        private async Task HandleAttachmentsMessage(IEnumerable<Attachment> attachments, string senderId)
        {
            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    if (attachment.Type == "location")
                    {
                        await HandleLocation(attachment, senderId);
                    }
                    else if (attachment.Type != null)
                    {
                        await SendBasicResponse(senderId);
                    }
                }
            }
        }

        private async Task SendBasicResponse(string senderId)
        {
            await _messengerClient.CallSendApi(senderId,
                "Please send your location in order to get nearby restaurants info");
        }

        private async Task HandleLocation(Attachment attachment, string senderId)
        {
            var nearby= await _zomatoClient.CallZomatoApi
            (attachment.Payload.Coordinates.Latitude,
                attachment.Payload.Coordinates.Longitude);

            await _messengerClient.CallSendApi(senderId,
                GetRestaurantsString(nearby.Restaurants));
        }

        private string GetRestaurantsString(ICollection<RestaurantWrapper> restaurants)
        {
            return restaurants.Aggregate("",
                (current, next) => current + (next.Restaurant.Name + "\n" + next.Restaurant.Location.Address +
                                              "\n" + next.Restaurant.Url + "\n\n"));
        }
        
    }
}