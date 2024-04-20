using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

internal class Program
{
    // this program emulates system which stores data about energy efficiency measures applied to
    // power transmission lines and their operating modes and allows for assessment of impact of those measures

    record Station(int Id, string Name);
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
        dbConnection.Execute("DELETE FROM Station;");
        dbConnection.Execute("DELETE FROM Line;");
    }

    private static (Station station1, Station station2) CreateStations(NpgsqlConnection dbConnection)
    {
        var station1 = new Station(0, "Station A");
        var station2 = new Station(0, "Station B");
        station1 = station1 with { Id = dbConnection.QuerySingle<int>("insert into Station (Name) values (@Name) returning Id;", station1) };
        station2 = station2 with { Id = dbConnection.QuerySingle<int>("insert into Station (Name) values (@Name) returning Id;", station2) };
        return (station1, station2);
    }

    private static void CreateLines(NpgsqlConnection dbConnection, Station station1, Station station2) => 
        dbConnection.Execute("insert into Line (StationId_1, StationId_2) values (@StationId_1, @StationId_2);",
            new Line(station1.Id, station2.Id));
}