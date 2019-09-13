using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FbRestaurantsBot.Core.Configuration;
using FbRestaurantsBot.Core.Dtos.Messaging;
using FbRestaurantsBot.Core.Exceptions;
using FbRestaurantsBot.Core.Helpers;
using FbRestaurantsBot.Core.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace FbRestaurantsBot.Infrastructure.Clients
{
    public class MessengerClient : IMessengerClient
    {
        private readonly HttpClient _httpClient;
        private readonly FacebookSettings _fbSettings;

        public MessengerClient(HttpClient httpClient, 
            IOptions<FacebookSettings> fbSettings)
        {
            _httpClient = httpClient;
            _fbSettings = fbSettings.Value;
        }

        public async Task CallSendApi(string senderId, string responseMessage)
        {
            var responseObj = new Response
            {
                Recipient = new Recipient
                {
                    Id = senderId
                },
                Message = new MessageResponse
                {
                    Text = responseMessage
                }
            };
            var stringPayload = JsonConvert.SerializeObject(responseObj);
            var content = new StringContent(stringPayload, Encoding.UTF8, Constants.ApplicationJson);
            var response = await _httpClient.PostAsync(
                "?access_token=" +
                $"{_fbSettings.Secret}", content);
            if(!response.IsSuccessStatusCode)
            {
                throw new ApiCallException(Constants.ApiCallExceptionMessage, (int) response.StatusCode,
                    response.ReasonPhrase);
            }
        } 
    }
}