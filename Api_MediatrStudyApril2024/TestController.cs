using Microsoft.AspNetCore.Mvc;

namespace Api_MediatrStudyApril2024;

[ApiController]
[Route("test")]  
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok("Welcome to my API!");
}
