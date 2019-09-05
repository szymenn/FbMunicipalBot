using System.Threading.Tasks;
using FbRestaurantsBot.Helpers;
using FbRestaurantsBot.Services;
using Microsoft.AspNetCore.Mvc;

namespace FbRestaurantsBot.Controllers
{
    [Route("webhook")]
    [ApiController]
    public class WebHookController : ControllerBase
    {
        private readonly IMessengerService _messengerService;

        public WebHookController(IMessengerService messengerService)
        {
            _messengerService = messengerService;
        }


        [HttpGet]
        public IActionResult Verify(
            [FromQuery(Name = Constants.QueryMode)]
            string mode,
            [FromQuery(Name = Constants.QueryChallenge)]
            string challenge,
            [FromQuery(Name = Constants.QueryVerifyToken)]
            string token)
        {
            _messengerService.VerifyToken(token, mode);
            return Ok(challenge);
        }


        [HttpPost]
        public async Task<IActionResult> Receive()
        {
            await _messengerService.Receive(Request);
            return Ok();
        }

    }
}