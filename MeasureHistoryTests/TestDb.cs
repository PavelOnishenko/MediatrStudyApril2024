using MeasureHistoryWebService.Db;
using MeasureHistoryWebService.Entities;

namespace MeasureHistoryTests;

internal class TestDb : IDb
{
    public List<measure_history> HistoryRecords { get; set; } = [];

    public void ClearTables() => HistoryRecords.Clear();

    public void Dispose() { }

    public IEnumerable<measure_history> GetHistoryRecordsForDay(DateTime theDay) => 
        HistoryRecords.Where(x => x.application_date.Date == theDay);

    public void SaveHistoryRecord(SaveHistoryRecordModel model)
    {
        var maxId = HistoryRecords.Any() ? HistoryRecords.Max(x => x.id) : 0;
        HistoryRecords.Add(new measure_history(
            maxId + 1, model.station_uuid, model.application_date, model.old_efficiency, model.new_efficiency));
    }
}
