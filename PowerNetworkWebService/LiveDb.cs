using Npgsql;
using Dapper;
using PowerNetworkWebService.Entities;

namespace PowerNetworkWebService
{
    public class LiveDb : IDb
    {
        private readonly string connectionString;
        private readonly NpgsqlConnection connection;

        public LiveDb(string connectionString)
        {
            this.connectionString = connectionString;
            connection = new NpgsqlConnection(connectionString);
            connection.Open();
        }

        public void ClearTables()
        {
            connection.Execute("DELETE FROM station;");
            connection.Execute("DELETE FROM line;");
        }

        public float GetAverageLoss()
        {
            var stations = connection.Query<station>("SELECT * FROM station;").ToArray();
            return stations.Average(x => x.efficiency);
        }

        public (IEnumerable<station> stations, IEnumerable<line> lines) SeedTestData()
        {
            ClearTables();
            var (station1, station2) = CreateStations(connection);
            var lines = CreateLines(connection, station1, station2);
            return (new[] { station1, station2 }, lines);
        }

        public void SetEfficiency(int stationId, float newEfficiency)
        {
            using var dbConnection = new NpgsqlConnection(connectionString);
            dbConnection.Open();
            dbConnection.Execute("UPDATE station SET efficiency = @newEfficiency WHERE id = @stationId;",
                new { stationId, newEfficiency });
        }

        private static (station station1, station station2) CreateStations(NpgsqlConnection dbConnection)
        {
            var station1 = new station(0, "Station A", 3);
            var station2 = new station(0, "Station B", 5);
            var stationInsertSql = "insert into station (name, efficiency) values (@Name, @efficiency) returning id;";
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

        public void Dispose()
        {
            connection.Close();
            connection.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
