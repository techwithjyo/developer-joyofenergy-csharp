using JOIEnergy.Domain;
using System.Collections.Generic;

namespace JOIEnergy.RepositoryFolder
{
    public class MeterReadingRepository : IMeterReadingRepository
    {
        private readonly Dictionary<string, List<ElectricityReading>> _meterAssociatedReadings;
        public MeterReadingRepository(Dictionary<string, List<ElectricityReading>> meterAssociatedReadings)
        {
            _meterAssociatedReadings = meterAssociatedReadings;
        }
        public Dictionary<string, List<ElectricityReading>> GetAllReadings()
        {
            return _meterAssociatedReadings;
        }

        public List<ElectricityReading> GetReading(string smartMeterId)
        {
            if (_meterAssociatedReadings.ContainsKey(smartMeterId))
            {
                return _meterAssociatedReadings[smartMeterId];
            }
            return new List<ElectricityReading>();
        }

        public void StoreReading(string smartMeterId, List<ElectricityReading> electricityReadings)
        {
            if (!_meterAssociatedReadings.ContainsKey(smartMeterId))
            {
                _meterAssociatedReadings.Add(smartMeterId, new List<ElectricityReading>());
            }

            electricityReadings.ForEach(electricityReading => _meterAssociatedReadings[smartMeterId].Add(electricityReading));

        }
    }
}
