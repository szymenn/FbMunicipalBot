using System.Threading.Tasks;
using FbMunicipalTransportBot.Services;
using Microsoft.AspNetCore.Mvc;

namespace FbMunicipalTransportBot.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IZomatoApiClient _zomatoApiClient;

        public TestController(IZomatoApiClient zomatoApiClient)
        {
            _zomatoApiClient = zomatoApiClient;
        }
        
        [HttpGet]
        public async Task<IActionResult> Test(double lat, double lon)
        {
            var restaurants = await _zomatoApiClient.CallZomatoApi(lat, lon);
            return Ok();
        } 
    }
}