using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerNetworkWebService.Commands;

namespace PowerNetworkWebService.Controllers.MeasureController;

[ApiController]
[Route("api/[controller]")]
[Authorize("writeAPI")]
public class MeasureController(ISender mediator) : ControllerBase
{
    private readonly ISender mediator = mediator;

    [HttpPost("apply-measure")]
    public async Task<IActionResult> ApplyMeasure([FromBody] ApplyMeasureRequestModel model)
    {
        await mediator.Send(new ApplyEfficiencyMeasuresCommand(model.StationId, model.NewEfficiency));
        return Ok();
    }
}