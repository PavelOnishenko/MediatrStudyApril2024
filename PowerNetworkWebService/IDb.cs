using PowerNetworkWebService.Entities;

namespace PowerNetworkWebService
{
    public interface IDb : IDisposable
    {
        void ClearTables();

        (IEnumerable<station> stations, IEnumerable<line> lines) SeedTestData();

        void SetEnergyLoss(int stationId,  float newEnergyLoss);

        float GetAverageLoss();
    }
}
