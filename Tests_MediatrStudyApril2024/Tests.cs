using Library_MediatrStudyApril2024;
using Library_MediatrStudyApril2024.Commands;
using Library_MediatrStudyApril2024.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Tests_MediatrStudyApril2024
{
    public class Tests
    {
        [Test]
        public void ApplyEfficiencyMeasures_MainScenario()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplyEfficiencyMeasuresHandler).Assembly));
            services.AddTransient<ApplyEfficiencyMeasuresHandler>();
            services.AddSingleton<IDb, TestDb>();
            var serviceProvider = services.BuildServiceProvider();
            var handler = serviceProvider.GetRequiredService<ApplyEfficiencyMeasuresHandler>();
            var db = serviceProvider.GetRequiredService<IDb>() as TestDb;
            db.CreateStation(new station(1, "Station 351", 5));
            var request = new ApplyEfficiencyMeasuresCommand(1, 4);

            handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            var stationAfterMeasure = db.GetStations().Single();
            Assert.That(stationAfterMeasure.id, Is.EqualTo(1));
            Assert.That(stationAfterMeasure.energy_loss, Is.EqualTo(4));
        }
    }
}