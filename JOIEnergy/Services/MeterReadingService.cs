using JOIEnergy.Domain;
using JOIEnergy.RepositoryFolder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JOIEnergy.Services
{
    public class MeterReadingService : IMeterReadingService
    {
        private readonly IMeterReadingRepository _meterReadingRepository;
        public MeterReadingService(IMeterReadingRepository meterReadingRepository)
        {
            _meterReadingRepository = meterReadingRepository;
        }

        public List<ElectricityReading> GetReadings(string smartMeterId)
        {
            return _meterReadingRepository.GetReading(smartMeterId);
        }

        public void StoreReadings(string smartMeterId, List<ElectricityReading> electricityReadings)
        {
            _meterReadingRepository.StoreReading(smartMeterId, electricityReadings);
        }

        public Dictionary<string, List<ElectricityReading>> GetAllReadings()
        {
            return _meterReadingRepository.GetAllReadings();
        }

        public decimal? AverageConsumptionPerDay(string smartMeterId)
        {
            var readings = _meterReadingRepository.GetReading(smartMeterId);
            if (!readings.Any())
            {
                return 0;
            }

            var groupedByDay = readings.GroupBy(r => r.Time.Date);
            var dailyAverages = groupedByDay.Select(g => g.Sum(r => r.Reading));
            var avg = dailyAverages.Average();
            return Math.Round(avg, 2);
        }

        public bool IsMeterReadingsValid(MeterReadings meterReadings)
        {
            string smartMeterId = meterReadings.SmartMeterId;
            List<ElectricityReading> electricityReadings = meterReadings.ElectricityReadings;
            return smartMeterId != null && smartMeterId.Any()
                    && electricityReadings != null && electricityReadings.Any();
        }
    }
}
