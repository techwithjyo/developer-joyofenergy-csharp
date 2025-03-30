using JOIEnergy.Domain;
using System.Collections.Generic;

namespace JOIEnergy.RepositoryFolder
{
    public class MeterReadingRepository : IMeterReadingRepository
    {
        private readonly Dictionary<string, List<ElectricityReading>> _readings;

        public MeterReadingRepository(Dictionary<string, List<ElectricityReading>> readings)
        {
            _readings = readings;
        }
        public List<ElectricityReading> GetReading(string smartMeterId)
        {
            return _readings.ContainsKey(smartMeterId) ? _readings[smartMeterId] : new List<ElectricityReading>();
        }

        public void StoreReading(string smartMeterId, List<ElectricityReading> electricityReadings)
        {
            if (_readings.ContainsKey(smartMeterId))
            {
                _readings[smartMeterId].AddRange(electricityReadings);
            }
            else
            {
                _readings[smartMeterId] = new List<ElectricityReading>(electricityReadings);
            }
        }

        public Dictionary<string, List<ElectricityReading>> GetAllReadings()
        {
            return _readings;
        }
    }
}
