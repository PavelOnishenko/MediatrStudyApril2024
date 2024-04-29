using Microsoft.Extensions.DependencyInjection;
using PowerNetworkWebService.Commands;
using PowerNetworkWebService.Db;
using PowerNetworkWebService.Entities;

namespace PowerNetworkTests;

public class Tests
{
    [Test]
    public void ApplyEfficiencyMeasure_MainScenario()
    {
        var serviceProvider = GetServiceProvider();
        var db = (serviceProvider.GetRequiredService<IDb>() as TestDb)!;
        var uuid = Guid.NewGuid();
        db.Stations.Add(new station(1, uuid, "Station 351", 5));

        serviceProvider.GetRequiredService<ApplyEfficiencyMeasureHandler>()
            .Handle(new ApplyEfficiencyMeasureCommand(1, 4), CancellationToken.None).GetAwaiter().GetResult();

        var stationAfterMeasure = db.Stations.Single();
        Assert.Multiple(() =>
        {
            Assert.That(stationAfterMeasure.uuid, Is.EqualTo(uuid));
            Assert.That(stationAfterMeasure.efficiency, Is.EqualTo(4));
        });
    }

    private static ServiceProvider GetServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplyEfficiencyMeasureHandler).Assembly));
        services.AddTransient<ApplyEfficiencyMeasureHandler>();
        services.AddSingleton<IDb, TestDb>();
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }
}