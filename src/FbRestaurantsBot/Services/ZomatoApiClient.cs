using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using FbRestaurantsBot.Configuration;
using FbRestaurantsBot.Exceptions;
using FbRestaurantsBot.Helpers;
using FbRestaurantsBot.Models.Restaurants;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace FbRestaurantsBot.Services
{
    public class ZomatoApiClient : IZomatoApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ZomatoSettings _zomatoSettings;

        public ZomatoApiClient
        (
            HttpClient httpClient,
            IOptions<ZomatoSettings> zomatoSettings
        )
        {
            _httpClient = httpClient;
            _zomatoSettings = zomatoSettings.Value;
        }

        public async Task<Nearby> CallZomatoApi(double latitude, double longitude)
        {
            _httpClient.DefaultRequestHeaders.Add("user-key", _zomatoSettings.ApiKey);
            var latString = latitude.ToString(Constants.StringConversionFormat, CultureInfo.InvariantCulture);
            var longString = longitude.ToString(Constants.StringConversionFormat, CultureInfo.InvariantCulture);
            var response =
                await _httpClient.GetAsync(
                    $"geocode?lat={latString}&lon={longString}");

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiCallException(Constants.ApiCallExceptionMessage, (int) response.StatusCode,
                    response.ReasonPhrase);
            }

            var nearby = await response.Content.ReadAsAsync<Nearby>();
            return nearby;
        }
            
    } 
}