using Microsoft.Extensions.DependencyInjection;
using PowerNetworkWebService.Commands;
using PowerNetworkWebService.Db;
using PowerNetworkWebService.Entities;

namespace PowerNetworkTests;

public class Tests
{
    [Test]
    public void ApplyEfficiencyMeasures_MainScenario()
    {
        var serviceProvider = GetServiceProvider();
        var db = (serviceProvider.GetRequiredService<IDb>() as TestDb)!;
        db.Stations.Add(new station(1, "Station 351", 5));

        serviceProvider.GetRequiredService<ApplyEfficiencyMeasuresHandler>()
            .Handle(new ApplyEfficiencyMeasuresCommand(1, 4), CancellationToken.None).GetAwaiter().GetResult();

        var stationAfterMeasure = db.Stations.Single();
        Assert.Multiple(() =>
        {
            Assert.That(stationAfterMeasure.id, Is.EqualTo(1));
            Assert.That(stationAfterMeasure.efficiency, Is.EqualTo(4));
        });
    }

    private static ServiceProvider GetServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplyEfficiencyMeasuresHandler).Assembly));
        services.AddTransient<ApplyEfficiencyMeasuresHandler>();
        services.AddSingleton<IDb, TestDb>();
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }
}