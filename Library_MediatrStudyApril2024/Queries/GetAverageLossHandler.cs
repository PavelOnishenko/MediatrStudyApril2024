using MediatR;

namespace Library_MediatrStudyApril2024.Queries;

public class GetAverageLossHandler(IDb db) : IRequestHandler<GetAverageLossQuery, float>
{
    private readonly IDb db = db;

    public Task<float> Handle(GetAverageLossQuery request, CancellationToken cancellationToken) => Task.Run(db.GetAverageLoss);
}