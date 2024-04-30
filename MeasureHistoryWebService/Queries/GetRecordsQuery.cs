using MediatR;

namespace MeasureHistoryWebService.Queries;

public record GetRecordsQuery(DateTime date) : IRequest<MeasureRecord[]>;