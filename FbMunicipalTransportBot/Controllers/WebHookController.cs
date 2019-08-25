using System;
using Microsoft.AspNetCore.Mvc;

namespace FbMunicipalTransportBot.Controllers
{
    [Route("webhook")]
    public class WebHookController : ControllerBase
    {
        [HttpGet("verify")]
        public IActionResult Verify(
            [FromQuery(Name = "hub.mode")] string mode,
            [FromQuery(Name = "hub.challenge")] string challenge,
            [FromQuery(Name = "hub.verify_token")] string token)
        {
            if (token.Equals("heh"))
            {
                return Ok(challenge);
            }

            return Forbid();
        }
        
        
        
    }
}