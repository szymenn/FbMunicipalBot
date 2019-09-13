using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using FbRestaurantsBot.Core.Configuration;
using FbRestaurantsBot.Core.Dtos.Restaurants;
using FbRestaurantsBot.Core.Exceptions;
using FbRestaurantsBot.Core.Helpers;
using FbRestaurantsBot.Core.Interfaces;
using Microsoft.Extensions.Options;

namespace FbRestaurantsBot.Infrastructure.Clients
{
    public class ZomatoClient : IZomatoClient
    {
        private readonly HttpClient _httpClient;
        private readonly ZomatoSettings _zomatoSettings;

        public ZomatoClient
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