using System.Collections.Generic;
using System.Threading.Tasks;
using FbMunicipalTransportBot.Models.Restaurants;

namespace FbMunicipalTransportBot.Services
{
    public interface IZomatoApiClient
    {
        Task<RestaurantsNearby> CallZomatoApi(double latitude, double longitude);
    }
}