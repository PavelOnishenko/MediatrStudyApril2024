using MediatR;

namespace PowerNetworkWebService.Commands;

public record ApplyEfficiencyMeasureCommand(int StationId, float newEfficiency) : IRequest;