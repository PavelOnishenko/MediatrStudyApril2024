using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

internal class Program
{

    // this program emulates system which stores data about energy efficiency measures applied to
    // power transmission lines and their operating modes and allows for assessment of impact of those measures

    record station(int id, string name, float energy_loss);
    record line(int station_id_1, int station_id_2);

    private static void Main()
    {
        var connectionString = LoadConnectionString();
        if (connectionString is null) return; 
        var (stations, lines) = SeedDbWithTestData(connectionString);
        var averageLossBeforeMeasures = GetAverageLoss(connectionString);
        var stationToApplyMeasures = stations.Single(x => x.name == "Station A");
        ApplyEfficiencyMeasures(connectionString, stationToApplyMeasures.id, stationToApplyMeasures.energy_loss - 0.5);
        var averageLossAfterMeasures = GetAverageLoss(connectionString);
        Console.WriteLine($"Average efficiency before was [{averageLossBeforeMeasures}], after became [{averageLossAfterMeasures}].");
        Console.WriteLine($"Diff is [{averageLossAfterMeasures - averageLossBeforeMeasures}]]");
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