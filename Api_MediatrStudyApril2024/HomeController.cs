using Microsoft.AspNetCore.Mvc;

namespace Api_MediatrStudyApril2024
{
    [ApiController]
    [Route("api")]  // Root of the API
    public class HomeController : ControllerBase
    {
        [HttpGet]  // Responds to HTTP GET requests
        public IActionResult Get()
        {
            return Ok("Welcome to my API!");
        }
    }
}
