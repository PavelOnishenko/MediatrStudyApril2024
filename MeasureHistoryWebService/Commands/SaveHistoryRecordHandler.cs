using MeasureHistoryWebService.Db;
using MediatR;

namespace MeasureHistoryWebService.Commands;

internal class SaveHistoryRecordHandler(IDb db) : IRequestHandler<SaveHistoryRecordCommand>
{
    public Task Handle(SaveHistoryRecordCommand request, CancellationToken cancellationToken) => 
        Task.Run(() =>
        {
            var model = new SaveHistoryRecordModel(
                request.station_uuid, request.application_date, request.old_efficiency, request.new_efficiency);
            db.SaveHistoryRecord(model);
        });
}