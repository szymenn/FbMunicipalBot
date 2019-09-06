using System.Threading.Tasks;

namespace FbRestaurantsBot.Services
{
    public interface IMessengerClient
    {
        Task CallSendApi(string senderId, string responseMessage);
    }
}