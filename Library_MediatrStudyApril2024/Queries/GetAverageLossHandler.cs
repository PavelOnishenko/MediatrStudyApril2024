using MediatR;

namespace Library_MediatrStudyApril2024.Queries;

public class GetAverageLossHandler : IRequestHandler<GetAverageLossQuery, float>
{
    private readonly IDb db;

    public GetAverageLossHandler(IDb db)
    {
        this.db = db;
    }

    public Task<float> Handle(GetAverageLossQuery request, CancellationToken cancellationToken) => Task.Run(db.GetAverageLoss);
}