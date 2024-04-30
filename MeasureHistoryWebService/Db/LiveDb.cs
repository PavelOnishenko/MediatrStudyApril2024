using MeasureHistoryWebService.Entities;
using Npgsql;
using Dapper;

namespace MeasureHistoryWebService.Db;

public class LiveDb : IDb
{
    private readonly NpgsqlConnection connection;

    public LiveDb(string connectionString)
    {
        connection = new NpgsqlConnection(connectionString);
        connection.Open();
    }

    public IEnumerable<measure_history> GetHistoryRecordsForDay(DateTime theDay) =>
        connection.Query<measure_history>("SELECT * FROM measure_history WHERE DATE(application_date) = @theDay;", new {theDay});

    public void SaveHistoryRecord(SaveHistoryRecordModel newRecord) =>
        connection.Execute(
            @"INSERT INTO measure_history (station_uuid, application_date, old_efficiency, new_efficiency) 
                VALUES (@station_uuid, @application_date, @old_efficiency, @new_efficiency)", newRecord);

    public void Dispose()
    {
        connection.Close();
        connection.Dispose();
        GC.SuppressFinalize(this);
    }

    public void ClearTables() => connection.Execute("DELETE FROM measure_history;");
}