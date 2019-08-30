using System.Threading.Tasks;
using FbRestaurantsBot.Models.Restaurants;

namespace FbRestaurantsBot.Services
{
    public interface IZomatoApiClient
    {
        Task<RestaurantsNearby> CallZomatoApi(double latitude, double longitude);
    }
}