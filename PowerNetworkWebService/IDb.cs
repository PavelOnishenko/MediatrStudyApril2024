using PowerNetworkWebService.Entities;

namespace PowerNetworkWebService
{
    public interface IDb : IDisposable
    {
        void ClearTables();

        (IEnumerable<station> stations, IEnumerable<line> lines) SeedTestData();

        void SetEfficiency(int stationId,  float newEfficiency);

        float GetAverageLoss();
    }
}
