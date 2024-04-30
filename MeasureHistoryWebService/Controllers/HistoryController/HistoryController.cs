using MeasureHistoryWebService.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeasureHistoryWebService.Controllers.HistoryController;

[ApiController]
[Route("api/[controller]")]
[Authorize("theAPI")]
public class HistoryController(ISender mediator) : ControllerBase
{
    private readonly ISender mediator = mediator;

    [HttpGet("get-records")]
    public async Task<IActionResult> GetRecords([FromQuery] DateTime date)
    {
        var result = await mediator.Send(new GetRecordsQuery(date));
        return Ok(result);
    }
}
