using MeasureHistoryWebService.Commands;
using MeasureHistoryWebService.Db;
using Microsoft.Extensions.DependencyInjection;

namespace MeasureHistoryTests;

public class Tests
{
    [Test]
    public void SaveHistoryRecord_MainScenario()
    {
        var serviceProvider = GetServiceProvider();

        serviceProvider.GetRequiredService<SaveHistoryRecordHandler>()
            .Handle(new SaveHistoryRecordCommand("CoolUuid", new DateTime(2024, 4, 24), 4, 5), CancellationToken.None)
            .GetAwaiter().GetResult();

        var db = (serviceProvider.GetRequiredService<IDb>() as TestDb)!;
        var recordFromDb = db.HistoryRecords.Single();
        Assert.Multiple(() =>
        {
            Assert.That(recordFromDb.id, Is.EqualTo(1));
            Assert.That(recordFromDb.station_uuid, Is.EqualTo("CoolUuid"));
            Assert.That(recordFromDb.application_date, Is.EqualTo(new DateTime(2024, 4, 24)));
            Assert.That(recordFromDb.old_efficiency, Is.EqualTo(4));
            Assert.That(recordFromDb.new_efficiency, Is.EqualTo(5));
        });
    }

    private static ServiceProvider GetServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SaveHistoryRecordHandler).Assembly));
        services.AddTransient<SaveHistoryRecordHandler>();
        services.AddSingleton<IDb, TestDb>();
        return services.BuildServiceProvider();
    }
}