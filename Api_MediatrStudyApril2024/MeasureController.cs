using Library_MediatrStudyApril2024.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api_MediatrStudyApril2024;

[ApiController]
[Route("api/[controller]")]
[Authorize("writeAPI")]
public class MeasureController(ISender mediator) : ControllerBase
{
    private readonly ISender mediator = mediator;

    [HttpPost("apply-measure")]
    public async Task<IActionResult> ApplyMeasure([FromBody] ApplyMeasureRequestModel model)
    {
        await mediator.Send(new ApplyEfficiencyMeasuresCommand(model.StationId, model.NewEnergyLoss));
        return Ok();
    }
}
