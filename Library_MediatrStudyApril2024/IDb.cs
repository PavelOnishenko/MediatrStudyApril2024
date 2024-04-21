using Library_MediatrStudyApril2024.Entities;

namespace Library_MediatrStudyApril2024
{
    public interface IDb : IDisposable
    {
        void ClearTables();

        (IEnumerable<station> stations, IEnumerable<line> lines) SeedTestData();

        void SetEnergyLoss(int stationId,  float newEnergyLoss);

        float GetAverageLoss();
    }
}
