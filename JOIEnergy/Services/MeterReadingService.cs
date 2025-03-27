using System;
using System.Collections.Generic;
using System.Linq;
using JOIEnergy.Domain;

namespace JOIEnergy.Services
{
    public class MeterReadingService : IMeterReadingService
    {
        public Dictionary<string, List<ElectricityReading>> MeterAssociatedReadings { get; set; }
        public MeterReadingService(Dictionary<string, List<ElectricityReading>> meterAssociatedReadings)
        {
            MeterAssociatedReadings = meterAssociatedReadings;
        }

        public List<ElectricityReading> GetReadings(string smartMeterId)
        {
            if (MeterAssociatedReadings.ContainsKey(smartMeterId))
            {
                return MeterAssociatedReadings[smartMeterId];
            }
            return new List<ElectricityReading>();
        }

        public void StoreReadings(string smartMeterId, List<ElectricityReading> electricityReadings)
        {
            if (!MeterAssociatedReadings.ContainsKey(smartMeterId))
            {
                MeterAssociatedReadings.Add(smartMeterId, new List<ElectricityReading>());
            }

            electricityReadings.ForEach(electricityReading => MeterAssociatedReadings[smartMeterId].Add(electricityReading));
        }

        public Dictionary<string, List<ElectricityReading>> GetAllReadings()
        {
            return MeterAssociatedReadings;
        }

        public decimal? AverageConsumptionPerDay(string smartMeterId)
        {
            if (MeterAssociatedReadings.ContainsKey(smartMeterId))
            {
                var reading = MeterAssociatedReadings[smartMeterId];
                //var avgReading = reading.Average(t => t.Reading);
                //return avgReading;
                var groupedByDay = reading.GroupBy(p => p.Time.Date);
                var dailyAverages = groupedByDay.Select(g => g.Sum(r => r.Reading));   
                return dailyAverages.Average();
            }  
            return 0;
        }
    }
}
