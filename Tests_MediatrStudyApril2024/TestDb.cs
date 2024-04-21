using Library_MediatrStudyApril2024;
using Library_MediatrStudyApril2024.Entities;

namespace Tests_MediatrStudyApril2024
{
    internal class TestDb : IDb
    {
        private List<station> Stations { get; set; } = new List<station>();

        private List<line> Lines { get; set; } = new List<line>();

        public List<station> GetStations() => Stations.ToList();

        public void CreateLine(line newLine)
        {
            Lines.Add(newLine);
        }

        public void CreateStation(station newStation)
        {
            Stations.Add(newStation);
        }

        public void ClearTables()
        {
            Stations.Clear();
            Lines.Clear();
        }

        public void Dispose()
        {
            
        }

        public float GetAverageLoss()
        {
            return Stations.Average(x => x.energy_loss);
        }

        public (IEnumerable<station> stations, IEnumerable<line> lines) SeedTestData()
        {
            throw new NotImplementedException();
        }

        public void SetEnergyLoss(int stationId, float newEnergyLoss)
        {
            var station = Stations.Single(x => x.id == stationId);
            Stations.Remove(station);
            Stations.Add(new station(stationId, station.name, newEnergyLoss));
        }
    }
}
