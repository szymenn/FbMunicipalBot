using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FbMunicipalTransportBot.Services
{
    public interface IMessengerService
    {
        Task Receive(HttpRequest request);
        void VerifyToken(string token, string mode);
    }
}