using PowerNetworkWebService;
using PowerNetworkWebService.Entities;

namespace Tests_MediatrStudyApril2024
{
    internal class TestDb : IDb
    {
        private List<station> Stations { get; set; } = [];

        private List<line> Lines { get; set; } = [];

        public List<station> GetStations() => [.. Stations];

        public void CreateLine(line newLine) => Lines.Add(newLine);

        public void CreateStation(station newStation) => Stations.Add(newStation);

        public void ClearTables()
        {
            Stations.Clear();
            Lines.Clear();
        }

        public void Dispose() { }

        public float GetAverageLoss() => Stations.Average(x => x.efficiency);

        public (IEnumerable<station> stations, IEnumerable<line> lines) SeedTestData() => throw new NotImplementedException();

        public void SetEnergyLoss(int stationId, float newEnergyLoss)
        {
            var station = Stations.Single(x => x.id == stationId);
            Stations.Remove(station);
            Stations.Add(new station(stationId, station.name, newEnergyLoss));
        }
    }
}
