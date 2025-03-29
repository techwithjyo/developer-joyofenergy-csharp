using System.Collections.Generic;
using System.Linq;
using JOIEnergy.Domain;

namespace JOIEnergy.Services;

public class HighUsageReadingService :IHighUsageService
{
    public Dictionary<string, List<ElectricityReading>> MeterAssociatedReadings;
    public HighUsageReadingService(Dictionary<string, List<ElectricityReading>> meterAssociatedReadings)
    {
        MeterAssociatedReadings = meterAssociatedReadings;
    }

    public ElectricityReading GetHighUsageReading(string meterId)
    {
        List<ElectricityReading> list = MeterAssociatedReadings[meterId];
        return list.OrderByDescending(x => x.Reading).FirstOrDefault();
    }
}