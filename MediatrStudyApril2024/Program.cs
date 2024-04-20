using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

internal class Program
{

    // this program emulates system which stores data about energy efficiency measures applied to
    // power transmission lines and their operating modes and allows for assessment of impact of those measures

    record Station(int Id, string Name, float EnergyLoss);
    record Line(int StationId_1, int StationId_2);

    private static void Main(string[] args)
    {
        var connectionString = LoadConnectionString();
        if (connectionString is not null) SeedDbWithTestData(connectionString);
    }

    private static string? LoadConnectionString() => 
        new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build()
        .GetConnectionString("PostgreSQLConnection");

    private static void SeedDbWithTestData(string connectionString)
    {
        using var dbConnection = new NpgsqlConnection(connectionString);
        dbConnection.Open();
        ClearTables(dbConnection);
        var stations = CreateStations(dbConnection);
        CreateLines(dbConnection, stations.station1, stations.station2);
    }

    private static void ClearTables(NpgsqlConnection dbConnection)
    {
        dbConnection.Execute("DELETE FROM station;");
        dbConnection.Execute("DELETE FROM line;");
    }

    private static (Station station1, Station station2) CreateStations(NpgsqlConnection dbConnection)
    {
        var station1 = new Station(0, "Station A", 3);
        var station2 = new Station(0, "Station B", 5);
        var stationInsertSql = "insert into station (name, energy_loss) values (@Name, @EnergyLoss) returning id;";
        station1 = station1 with { Id = dbConnection.QuerySingle<int>(stationInsertSql, station1) };
        station2 = station2 with { Id = dbConnection.QuerySingle<int>(stationInsertSql, station2) };
        return (station1, station2);
    }

    private static void CreateLines(NpgsqlConnection dbConnection, Station station1, Station station2) => 
        dbConnection.Execute("insert into Line (station_id_1, station_id_2) values (@StationId_1, @StationId_2);",
            new Line(station1.Id, station2.Id));

    private static float GetEfficiency(string connectionString)
    {
        using var dbConnection = new NpgsqlConnection(connectionString);
        dbConnection.Open();
        var stations = dbConnection.Query<Station>("SELECT * FROM station;").ToArray();
        return stations.Average(x => x.EnergyLoss);
    }

    private static void ApplyEfficiencyMeasures(string connectionString, int stationId, double newEnergyLoss)
    {
        using var dbConnection = new NpgsqlConnection(connectionString);
        dbConnection.Open();
        dbConnection.Execute("UPDATE station SET energyLoss = @newEnergyLoss WHERE id = @stationId;",
            new { stationId, newEnergyLoss });
        Console.WriteLine($"Updated Station {stationId} to have an energy loss of {newEnergyLoss}%.");
    }
}