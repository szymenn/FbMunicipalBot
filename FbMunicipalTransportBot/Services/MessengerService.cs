using System;
using System.IO;
using System.Threading.Tasks;
using FbMunicipalTransportBot.Configuration;
using FbMunicipalTransportBot.Exceptions;
using FbMunicipalTransportBot.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VerificationException = System.Security.VerificationException;

namespace FbMunicipalTransportBot.Services
{
    public class MessengerService : IMessengerService
    {
        private readonly FacebookSettings _fbSettings;
        private readonly IMessengerClient _messengerClient;
        private readonly ILogger<MessengerService> _logger;

        public MessengerService(IOptions<FacebookSettings> fbSettings, 
            IMessengerClient messengerClient, 
            ILogger<MessengerService> logger)
        {
            _fbSettings = fbSettings.Value;
            _messengerClient = messengerClient;
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
                    await _messengerClient.CallSendApi(webHookEvent.Sender.Id, "hehe odpowiadam");
                }
                if (webHookEvent.Postback != null)
                {
                    throw new NotImplementedException();
                }
                
            }
            
        }
    }
}