using MediatR;

namespace Library_MediatrStudyApril2024.Commands;

internal class ApplyEfficiencyMeasuresHandler : IRequestHandler<ApplyEfficiencyMeasuresCommand>
{
    private readonly IDb db;

    public ApplyEfficiencyMeasuresHandler(IDb db)
    {
        this.db = db;
    }

    public Task Handle(ApplyEfficiencyMeasuresCommand request, CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            db.SetEnergyLoss(request.StationId, request.NewEnergyLoss);
            Console.WriteLine($"Updated Station {request.StationId} to have an energy loss of {request.NewEnergyLoss}%.");
        }, cancellationToken);
    }
}