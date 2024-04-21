using MediatR;

namespace Library_MediatrStudyApril2024.Queries;

public record GetAverageLossQuery(string ConnectionString) : IRequest<float>;
