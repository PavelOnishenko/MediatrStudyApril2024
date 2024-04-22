using MediatR;

namespace PowerNetworkWebService.Commands;

public record ApplyEfficiencyMeasuresCommand(int StationId, float NewEnergyLoss) : IRequest;