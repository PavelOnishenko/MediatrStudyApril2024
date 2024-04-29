using MediatR;
using PowerNetworkWebService.Db;

namespace PowerNetworkWebService.Commands;

internal class ApplyEfficiencyMeasureHandler(IDb db) : IRequestHandler<ApplyEfficiencyMeasureCommand>
{
    private readonly IDb db = db;

    public Task Handle(ApplyEfficiencyMeasureCommand request, CancellationToken cancellationToken) =>
        Task.Run(() => db.SetEfficiency(request.StationId, request.newEfficiency), cancellationToken);
}