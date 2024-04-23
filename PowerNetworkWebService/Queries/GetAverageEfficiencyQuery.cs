using MediatR;

namespace PowerNetworkWebService.Queries;

public record GetAverageEfficiencyQuery() : IRequest<float>;