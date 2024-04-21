using Library_MediatrStudyApril2024.Entities;
using MediatR;

namespace Library_MediatrStudyApril2024.Commands;

public class SeedDbWithTestDataHandler : IRequestHandler<SeedDbWithTestDataCommand, (IEnumerable<station> stations, IEnumerable<line> lines)>
{
    private readonly IDb db;

    public SeedDbWithTestDataHandler(IDb db)
    {
        this.db = db;
    }

    public Task<(IEnumerable<station> stations, IEnumerable<line> lines)> Handle(SeedDbWithTestDataCommand request, CancellationToken cancellationToken) => 
        Task.Run(db.SeedTestData);
}