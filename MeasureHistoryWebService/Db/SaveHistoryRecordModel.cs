namespace MeasureHistoryWebService.Db;

public record SaveHistoryRecordModel(string station_uuid, DateTime application_date, float old_efficiency, float new_efficiency);