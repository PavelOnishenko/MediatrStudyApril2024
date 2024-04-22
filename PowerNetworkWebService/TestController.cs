using Microsoft.AspNetCore.Mvc;

namespace PowerNetworkWebService;

[ApiController]
[Route("test")]  
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok("Welcome to my API!");
}
