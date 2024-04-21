using MediatR;
using Npgsql;
using Dapper;

namespace Library_MediatrStudyApril2024.Commands;

public class ApplyEfficiencyMeasuresHandler : IRequestHandler<ApplyEfficiencyMeasuresCommand>
{
    public Task Handle(ApplyEfficiencyMeasuresCommand request, CancellationToken cancellationToken) =>
        Task.Run(() => ApplyEfficiencyMeasures(request.ConnectionString, request.StationId, request.NewEnergyLoss), cancellationToken);

    private static void ApplyEfficiencyMeasures(string connectionString, int stationId, double newEnergyLoss)
    {
        using var dbConnection = new NpgsqlConnection(connectionString);
        dbConnection.Open();
        dbConnection.Execute("UPDATE station SET energy_loss = @newEnergyLoss WHERE id = @stationId;",
            new { stationId, newEnergyLoss });
        Console.WriteLine($"Updated Station {stationId} to have an energy loss of {newEnergyLoss}%.");
    }
}