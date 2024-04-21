using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Library_MediatrStudyApril2024.Commands;
using Library_MediatrStudyApril2024.Queries;
using Library_MediatrStudyApril2024.Entities;
using Library_MediatrStudyApril2024;

internal partial class Program
{

    // this program emulates system which stores data about energy efficiency measures applied to
    // power transmission lines and their operating modes and allows for assessment of impact of those measures

#pragma warning disable CA1859 // Use concrete types when possible for improved performance
    private static IServiceProvider ConfigureServices()
#pragma warning restore CA1859 // Use concrete types when possible for improved performance
    {
        IServiceCollection services = new ServiceCollection();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(station).Assembly));
        services.AddTransient<IDb, LiveDb>(provider => new LiveDb(GetConnectionString()));
        return services.BuildServiceProvider();
    }

    static async Task Main()
    {
        var mediator = ConfigureServices().GetRequiredService<IMediator>();
        var connectionString = GetConnectionString();
        if (connectionString is null) return;
        var (stations, _) = await mediator.Send(new SeedDbWithTestDataCommand());
        var averageLossBeforeMeasures = await mediator.Send(new GetAverageLossQuery());
        var stationToApplyMeasures = stations.First();
        await mediator.Send(new ApplyEfficiencyMeasuresCommand(stationToApplyMeasures.id, stationToApplyMeasures.energy_loss - 0.5f));
        var averageLossAfterMeasures = await mediator.Send(new GetAverageLossQuery());

        Console.WriteLine($"Average efficiency before was [{averageLossBeforeMeasures}], after became [{averageLossAfterMeasures}].");
        Console.WriteLine($"Diff is [{averageLossAfterMeasures - averageLossBeforeMeasures}]");
    }

    private static string? GetConnectionString() =>
        GetConfiguration().GetConnectionString("PostgreSQLConnection");

    private static IConfigurationRoot GetConfiguration()
    {
        return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
    }
}