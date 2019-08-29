using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using FbMunicipalTransportBot.Configuration;
using FbMunicipalTransportBot.Models.Restaurants;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace FbMunicipalTransportBot.Services
{
    public class ZomatoApiClient : IZomatoApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ZomatoSettings _zomatoSettings;
        private ILogger<ZomatoApiClient> _logger;

        public ZomatoApiClient
        (
            HttpClient httpClient,
            IOptions<ZomatoSettings> zomatoSettings,
            ILogger<ZomatoApiClient> logger
        )
        {
            _httpClient = httpClient;
            _zomatoSettings = zomatoSettings.Value;
            _logger = logger;
        }

        public async Task<RestaurantsNearby> CallZomatoApi(double latitude, double longitude)
        {
            _httpClient.DefaultRequestHeaders.Add("user-key", _zomatoSettings.ApiKey);
            var latString = latitude.ToString("0.0000", CultureInfo.InvariantCulture);
            var longString = longitude.ToString("0.0000", CultureInfo.InvariantCulture);
            var response =
                await _httpClient.GetAsync(
                    $"https://developers.zomato.com/api/v2.1/geocode?lat={latString}&lon={longString}");


            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Api call failed");
            }

            var restaurantsJsonString = await response.Content.ReadAsStringAsync();
            var restaurants = JsonConvert.DeserializeObject<RestaurantsNearby>(restaurantsJsonString);
            
            _logger.LogInformation("something has happpend");

            return null;
        }
            
    } 
}