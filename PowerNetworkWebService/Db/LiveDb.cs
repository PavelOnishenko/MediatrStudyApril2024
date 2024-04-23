using Npgsql;
using Dapper;
using PowerNetworkWebService.Entities;

namespace PowerNetworkWebService.Db;

public class LiveDb : IDb
{
    private readonly NpgsqlConnection connection;

    public LiveDb(string connectionString)
    {
        connection = new NpgsqlConnection(connectionString);
        connection.Open();
    }

    public void ClearTables()
    {
        connection.Execute("DELETE FROM station;");
        connection.Execute("DELETE FROM line;");
    }

    public float GetAverageEfficiency() => connection.Query<station>("SELECT * FROM station;").Average(x => x.efficiency);

    public void SetEfficiency(int stationId, float newEfficiency) => 
        connection.Execute("UPDATE station SET efficiency = @newEfficiency WHERE id = @stationId;",
            new { stationId, newEfficiency });

    public IEnumerable<station> SeedTestData()
    {
        ClearTables();
        return CreateTestStations(connection);
    }

    private static station[] CreateTestStations(NpgsqlConnection dbConnection)
    {
        var station1 = new station(0, "Station A", 3);
        var station2 = new station(0, "Station B", 5);
        var stationInsertSql = "insert into station (name, efficiency) values (@Name, @efficiency) returning id;";
        station1 = station1 with { id = dbConnection.QuerySingle<int>(stationInsertSql, station1) };
        station2 = station2 with { id = dbConnection.QuerySingle<int>(stationInsertSql, station2) };
        return [station1, station2];
    }

    public void Dispose()
    {
        connection.Close();
        connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
