using Library_MediatrStudyApril2024.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api_MediatrStudyApril2024
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeasureController(ISender mediator) : Controller
    {
        private readonly ISender mediator = mediator;

        [HttpPost("apply-measure")]
        public async Task<IActionResult> ApplyMeasure(ApplyMeasureRequestModel model)
        {
            var command = new ApplyEfficiencyMeasuresCommand(model.StationId, model.NewEnergyLoss);
            await mediator.Send(command);
            return Ok();
        }
    }
}
