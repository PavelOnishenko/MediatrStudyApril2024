using MediatR;
using PowerNetworkWebService.Db;

namespace PowerNetworkWebService.Queries;

public class GetAverageEfficiencyHandler(IDb db) : IRequestHandler<GetAverageEfficiencyQuery, float>
{
    private readonly IDb db = db;

    public Task<float> Handle(GetAverageEfficiencyQuery request, CancellationToken cancellationToken) => 
        Task.Run(db.GetAverageEfficiency);
}