using Dapper;
using MediatR;
using Npgsql;

namespace Library_MediatrStudyApril2024.Commands;

public class SeedDbWithTestDataHandler : IRequestHandler<SeedDbWithTestDataCommand, (IEnumerable<station> stations, IEnumerable<line> lines)>
{
    public Task<(IEnumerable<station> stations, IEnumerable<line> lines)> Handle(SeedDbWithTestDataCommand request, CancellationToken cancellationToken) =>
        Task.Run(() => SeedDbWithTestData(request.ConnectionString));

    private static (IEnumerable<station> stations, IEnumerable<line> lines) SeedDbWithTestData(string connectionString)
    {
        using var dbConnection = new NpgsqlConnection(connectionString);
        dbConnection.Open();
        ClearTables(dbConnection);
        var (station1, station2) = CreateStations(dbConnection);
        var lines = CreateLines(dbConnection, station1, station2);
        return (new[] { station1, station2 }, lines);
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

    private static void ClearTables(NpgsqlConnection dbConnection)
    {
        dbConnection.Execute("DELETE FROM station;");
        dbConnection.Execute("DELETE FROM line;");
    }
}