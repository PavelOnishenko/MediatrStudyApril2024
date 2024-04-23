namespace PowerNetworkWebService.Db;

public interface IDb : IDisposable
{
    void SetEfficiency(int stationId, float newEfficiency);

    float GetAverageEfficiency();

    void ClearTables();
}