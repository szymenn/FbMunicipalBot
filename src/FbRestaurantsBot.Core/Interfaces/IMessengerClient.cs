using System.Threading.Tasks;

namespace FbRestaurantsBot.Core.Interfaces
{
    public interface IMessengerClient
    {
        Task CallSendApi(string senderId, string responseMessage);
    }
}