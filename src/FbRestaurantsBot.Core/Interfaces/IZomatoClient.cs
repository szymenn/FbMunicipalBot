using System.Threading.Tasks;
using FbRestaurantsBot.Core.Dtos.Restaurants;

namespace FbRestaurantsBot.Core.Interfaces
{
    public interface IZomatoClient
    {
        Task<Nearby> CallZomatoApi(double latitude, double longitude);
    }
}