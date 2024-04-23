using MeasureHistoryWebService.Entities;

namespace MeasureHistoryWebService.Db;

public interface IDb : IDisposable
{
    void SaveHistoryRecord(SaveHistoryRecordModel model);

    IEnumerable<measure_history> GetHistoryRecordsForDay(DateTime theDay);

    void ClearTables();
}