using MediatR;

namespace Library_MediatrStudyApril2024.Commands;

internal class ApplyEfficiencyMeasuresHandler(IDb db) : IRequestHandler<ApplyEfficiencyMeasuresCommand>
{
    private readonly IDb db = db;

    public Task Handle(ApplyEfficiencyMeasuresCommand request, CancellationToken cancellationToken) =>
        Task.Run(() => db.SetEnergyLoss(request.StationId, request.NewEnergyLoss), cancellationToken);
}