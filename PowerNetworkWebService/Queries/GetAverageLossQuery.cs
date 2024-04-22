using MediatR;

namespace PowerNetworkWebService.Queries;

public record GetAverageLossQuery() : IRequest<float>;