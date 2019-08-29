using System.Threading.Tasks;
using FbMunicipalTransportBot.Models.Messaging;

namespace FbMunicipalTransportBot.Services
{
    public interface IMessengerClient
    {
        Task CallSendApi(string senderId, string response);
    }
}