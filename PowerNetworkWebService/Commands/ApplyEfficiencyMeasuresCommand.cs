using MediatR;

namespace PowerNetworkWebService.Commands;

public record ApplyEfficiencyMeasuresCommand(int StationId, float newEfficiency) : IRequest;