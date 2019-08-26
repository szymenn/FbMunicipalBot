using System.IO;
using FbMunicipalTransportBot.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FbMunicipalTransportBot.Controllers
{
    [Route("webhook")]
    public class WebHookController : ControllerBase
    {
        private readonly ILogger<WebHookController> _logger;

        public WebHookController(ILogger<WebHookController> logger)
        {
            _logger = logger;
        }
        
        
        [HttpGet]
        public IActionResult Verify(
            [FromQuery(Name = "hub.mode")] string mode,
            [FromQuery(Name = "hub.challenge")] string challenge,
            [FromQuery(Name = "hub.verify_token")] string token)
        {
            if (mode != null && token != null)
            {
                if (mode == "subscribe" && token == "heh")
                {
                    _logger.LogInformation("WEBHOOK_VERIFIED");
                    return Ok(challenge);
                }
            }

            return StatusCode(403);
        }


        [HttpPost]
        public IActionResult Receive()
        {
            using (var reader = new StreamReader(Request.Body))
            {
                var body = reader.ReadToEnd();
                var requestObject = JsonConvert.DeserializeObject<WebHookRequest>(body);
                if (requestObject.Object == "page")
                {
                    foreach (var entry in requestObject.Entry)
                    {
                        var webHookEvent = entry.Messaging[0];
                        _logger.LogInformation($"event message: {webHookEvent.Message.Text} ");
                    }

                    return Ok("EVENT_RECEIVED");
                }
            }
            

            return BadRequest();
        }

        
        
    }
}