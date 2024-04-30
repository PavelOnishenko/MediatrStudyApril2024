using MeasureHistoryWebService.Db;
using MediatR;

namespace MeasureHistoryWebService.Queries;

public class GetRecordsHandler(IDb db) : IRequestHandler<GetRecordsQuery, MeasureRecord[]>
{
    private readonly IDb db = db;

    Task<MeasureRecord[]> IRequestHandler<GetRecordsQuery, MeasureRecord[]>.Handle(GetRecordsQuery request, CancellationToken cancellationToken) =>
        Task.Run(() =>
        {
            return db.GetHistoryRecordsForDay(request.date)
                .Select(x => new MeasureRecord(x.station_uuid, x.application_date, x.old_efficiency, x.new_efficiency)).ToArray();
        });
}