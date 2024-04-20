using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

internal class Program
{

    // this program emulates system which stores data about energy efficiency measures applied to
    // power transmission lines and their operating modes and allows for assessment of impact of those measures

#pragma warning disable CA1859 // Use concrete types when possible for improved performance
    private static IServiceProvider ConfigureServices()
#pragma warning restore CA1859 // Use concrete types when possible for improved performance
    {
        IServiceCollection services = new ServiceCollection();

        services.AddMediatR(cfg =>
             cfg.RegisterServicesFromAssembly(typeof(station).Assembly));
        return services.BuildServiceProvider();
    }

    public record SeedDbWithTestDataCommand(string ConnectionString) : IRequest<(IEnumerable<station> stations, IEnumerable<line> lines)>;
    public record ApplyEfficiencyMeasuresCommand(string ConnectionString, int StationId, float NewEnergyLoss) : IRequest;
    public record GetAverageLossQuery(string ConnectionString) : IRequest<float>;

    public class SeedDbWithTestDataHandler : IRequestHandler<SeedDbWithTestDataCommand, (IEnumerable<station> stations, IEnumerable<line> lines)>
    {`
        public Task<(IEnumerable<station> stations, IEnumerable<line> lines)> Handle(SeedDbWithTestDataCommand request, CancellationToken cancellationToken) =>
            Task.Run(() => SeedDbWithTestData(request.ConnectionString));
    }

    public class ApplyEfficiencyMeasuresHandler : IRequestHandler<ApplyEfficiencyMeasuresCommand>
    {
        public Task Handle(ApplyEfficiencyMeasuresCommand request, CancellationToken cancellationToken) =>
            Task.Run(() => ApplyEfficiencyMeasures(request.ConnectionString, request.StationId, request.NewEnergyLoss), cancellationToken);
    }

    public class GetAverageLossHandler : IRequestHandler<GetAverageLossQuery, float>
    {
        public Task<float> Handle(GetAverageLossQuery request, CancellationToken cancellationToken) =>
            Task.Run(() => GetAverageLoss(request.ConnectionString));
    }

    public record station(int id, string name, float energy_loss);
    public record line(int station_id_1, int station_id_2);

    static async Task Main()
    {
        var mediator = ConfigureServices().GetRequiredService<IMediator>();
        var connectionString = LoadConnectionString();
        if (connectionString is null) return;

        var (stations, lines) = await mediator.Send(new SeedDbWithTestDataCommand(connectionString));
        var averageLossBeforeMeasures = await mediator.Send(new GetAverageLossQuery(connectionString));
        var stationToApplyMeasures = stations.Single(x => x.name == "Station A");
        await mediator.Send(new ApplyEfficiencyMeasuresCommand(connectionString, stationToApplyMeasures.id, stationToApplyMeasures.energy_loss - 0.5f));
        var averageLossAfterMeasures = await mediator.Send(new GetAverageLossQuery(connectionString));

        Console.WriteLine($"Average efficiency before was [{averageLossBeforeMeasures}], after became [{averageLossAfterMeasures}].");
        Console.WriteLine($"Diff is [{averageLossAfterMeasures - averageLossBeforeMeasures}]");
    }

    private static string? LoadConnectionString() => 
        new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build()
        .GetConnectionString("PostgreSQLConnection");

    private static (IEnumerable<station> stations, IEnumerable<line> lines) SeedDbWithTestData(string connectionString)
    {
        using var dbConnection = new NpgsqlConnection(connectionString);
        dbConnection.Open();
        ClearTables(dbConnection);
        var (station1, station2) = CreateStations(dbConnection);
        var lines = CreateLines(dbConnection, station1, station2);
        return (new[] { station1, station2 }, lines);
    }

    private static void ClearTables(NpgsqlConnection dbConnection)
    {
        dbConnection.Execute("DELETE FROM station;");
        dbConnection.Execute("DELETE FROM line;");
    }

    private static (station station1, station station2) CreateStations(NpgsqlConnection dbConnection)
    {
        var station1 = new station(0, "Station A", 3);
        var station2 = new station(0, "Station B", 5);
        var stationInsertSql = "insert into station (name, energy_loss) values (@Name, @energy_loss) returning id;";
        station1 = station1 with { id = dbConnection.QuerySingle<int>(stationInsertSql, station1) };
        station2 = station2 with { id = dbConnection.QuerySingle<int>(stationInsertSql, station2) };
        return (station1, station2);
    }

    private static line[] CreateLines(NpgsqlConnection dbConnection, station station1, station station2)
    {
        line param = new(station1.id, station2.id);
        dbConnection.Execute("insert into line (station_id_1, station_id_2) values (@station_id_1, @station_id_2);", param);
        return [param];
    }

    private static float GetAverageLoss(string connectionString)
    {
        using var dbConnection = new NpgsqlConnection(connectionString);
        dbConnection.Open();
        var stations = dbConnection.Query<station>("SELECT * FROM station;").ToArray();
        return stations.Average(x => x.energy_loss);
    }

    private static void ApplyEfficiencyMeasures(string connectionString, int stationId, double newEnergyLoss)
    {
        using var dbConnection = new NpgsqlConnection(connectionString);
        dbConnection.Open();
        dbConnection.Execute("UPDATE station SET energy_loss = @newEnergyLoss WHERE id = @stationId;",
            new { stationId, newEnergyLoss });
        Console.WriteLine($"Updated Station {stationId} to have an energy loss of {newEnergyLoss}%.");
    }
}