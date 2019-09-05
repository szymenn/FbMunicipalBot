using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FbRestaurantsBot.Configuration;
using FbRestaurantsBot.Exceptions;
using FbRestaurantsBot.Helpers;
using FbRestaurantsBot.Models.Messaging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace FbRestaurantsBot.Services
{
    public class MessengerClient : IMessengerClient
    {
        private readonly HttpClient _httpClient;
        private readonly FacebookSettings _fbSettings;
        private readonly ILogger<MessengerClient> _logger;

        public MessengerClient(HttpClient httpClient, 
            IOptions<FacebookSettings> fbSettings, 
            ILogger<MessengerClient> logger)
        {
            _httpClient = httpClient;
            _fbSettings = fbSettings.Value;
            _logger = logger;
        }


        public async Task CallSendApi(string senderId, string response)
        {
            var responseObj = new Response
            {
                Recipient = new Recipient
                {
                    Id = senderId
                },
                Message = new MessageResponse
                {
                    Text = response
                }
            };
            var stringPayload = JsonConvert.SerializeObject(responseObj);
            var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync(
                Constants.FacebookApiBaseUrl +
                $"{_fbSettings.Secret}", content);
            if (!result.IsSuccessStatusCode)
            {
                throw new MessengerException("Unable to send message");
            }
        } 
    }
}