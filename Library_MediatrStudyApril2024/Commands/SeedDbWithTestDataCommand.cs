using Library_MediatrStudyApril2024.Entities;
using MediatR;

namespace Library_MediatrStudyApril2024.Commands;

public record SeedDbWithTestDataCommand() : IRequest<(IEnumerable<station> stations, IEnumerable<line> lines)>;