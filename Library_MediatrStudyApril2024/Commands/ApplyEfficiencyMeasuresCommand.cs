using MediatR;

namespace Library_MediatrStudyApril2024.Commands;

public record ApplyEfficiencyMeasuresCommand(int StationId, float NewEnergyLoss) : IRequest;