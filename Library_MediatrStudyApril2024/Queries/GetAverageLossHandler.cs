using MediatR;
using Npgsql;
using Dapper;

namespace Library_MediatrStudyApril2024.Queries;

public class GetAverageLossHandler : IRequestHandler<GetAverageLossQuery, float>
{
    public Task<float> Handle(GetAverageLossQuery request, CancellationToken cancellationToken) =>
        Task.Run(() => GetAverageLoss(request.ConnectionString));

    private static float GetAverageLoss(string connectionString)
    {
        using var dbConnection = new NpgsqlConnection(connectionString);
        dbConnection.Open();
        var stations = dbConnection.Query<station>("SELECT * FROM station;").ToArray();
        return stations.Average(x => x.energy_loss);
    }
}