namespace MeasureHistoryWebService.Entities;

public record measure_history(int id, string station_uuid, DateTime application_date, float old_efficiency, float new_efficiency);