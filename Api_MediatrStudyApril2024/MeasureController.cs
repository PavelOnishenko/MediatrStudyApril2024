using Library_MediatrStudyApril2024.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api_MediatrStudyApril2024
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize("writeAPI")]
    public class MeasureController(ISender mediator) : ControllerBase
    {
        private readonly ISender mediator = mediator;

        [HttpPost("apply-measure")]
        public async Task<IActionResult> ApplyMeasure([FromBody] ApplyMeasureRequestModel model)
        {
            var command = new ApplyEfficiencyMeasuresCommand(model.StationId, model.NewEnergyLoss);
            await mediator.Send(command);
            return Ok();
        }
    }
}
