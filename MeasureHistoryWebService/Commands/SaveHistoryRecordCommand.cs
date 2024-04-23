using MediatR;

namespace MeasureHistoryWebService.Commands;

public record SaveHistoryRecordCommand(
    string station_uuid, DateTime application_date, float old_efficiency, float new_efficiency) : IRequest;