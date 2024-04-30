namespace MeasureHistoryWebService.Queries
{
    public record MeasureRecord(Guid station_uuid, DateTime application_date, float old_efficiency, float new_efficiency);
}
