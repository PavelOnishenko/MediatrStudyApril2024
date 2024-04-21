using MediatR;

namespace Library_MediatrStudyApril2024.Commands;

public record SeedDbWithTestDataCommand(string ConnectionString) : IRequest<(IEnumerable<station> stations, IEnumerable<line> lines)>;