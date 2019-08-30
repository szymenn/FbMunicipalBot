using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FbRestaurantsBot.Services
{
    public interface IMessengerService
    {
        Task Receive(HttpRequest request);
        void VerifyToken(string token, string mode);
    }
}