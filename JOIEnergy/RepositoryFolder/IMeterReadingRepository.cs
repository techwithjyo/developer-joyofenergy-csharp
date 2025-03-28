using JOIEnergy.Domain;
using System.Collections.Generic;

namespace JOIEnergy.RepositoryFolder
{
    public interface IMeterReadingRepository
    {
        List<ElectricityReading> GetReading(string smartMeterId);
        void StoreReading(string smartMeterId, List<ElectricityReading> electricityReadings);
        Dictionary<string, List<ElectricityReading>> GetAllReadings();
    }
}
