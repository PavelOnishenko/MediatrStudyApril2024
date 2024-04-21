using MediatR;

namespace Library_MediatrStudyApril2024.Commands;

public record ApplyEfficiencyMeasuresCommand(string ConnectionString, int StationId, float NewEnergyLoss) : IRequest;