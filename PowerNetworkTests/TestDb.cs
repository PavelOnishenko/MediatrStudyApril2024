using PowerNetworkWebService.Db;
using PowerNetworkWebService.Entities;

namespace PowerNetworkTests
{
    internal class TestDb : IDb
    {
        public List<station> Stations { get; set; } = [];

        public List<line> Lines { get; set; } = [];

        public void ClearTables()
        {
            Stations.Clear();
            Lines.Clear();
        }

        public void Dispose() { }

        public float GetAverageEfficiency() => Stations.Average(x => x.efficiency);

        public void SetEfficiency(int stationId, float newEfficiency)
        {
            var station = Stations.Single(x => x.id == stationId);
            Stations.Remove(station);
            Stations.Add(new station(stationId, station.name, newEfficiency));
        }
    }
}
