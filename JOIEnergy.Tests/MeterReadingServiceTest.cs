using JOIEnergy.Domain;
using JOIEnergy.RepositoryFolder;
using JOIEnergy.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace JOIEnergy.Tests
{
    public class MeterReadingServiceTest
    {
        private static string SMART_METER_ID = "smart-meter-id";

        private MeterReadingService meterReadingService;
        private MeterReadingService meterReadingServiceForDailyAvgConsumption;

        public MeterReadingServiceTest()
        {
            var meterReadingRepository = new MeterReadingRepository(new Dictionary<string, List<ElectricityReading>>());
            meterReadingService = new MeterReadingService(meterReadingRepository);
            meterReadingServiceForDailyAvgConsumption = new MeterReadingService(meterReadingRepository);

            meterReadingService.StoreReadings(SMART_METER_ID, new List<ElectricityReading>() {
        new ElectricityReading() { Time = DateTime.Now.AddMinutes(-30), Reading = 35m },
        new ElectricityReading() { Time = DateTime.Now.AddMinutes(-15), Reading = 30m }
    });
            meterReadingServiceForDailyAvgConsumption.StoreReadings(SMART_METER_ID, new List<ElectricityReading>() {
        new ElectricityReading() { Time = DateTime.Now.AddDays(-2), Reading = 35m },
        new ElectricityReading() { Time = DateTime.Now.AddDays(-1), Reading = 30m },
        new ElectricityReading() { Time = DateTime.Now, Reading = 25m }
    });
        }

        [Fact]
        public void GivenMeterIdThatDoesNotExistShouldReturnNull()
        {
            Assert.Empty(meterReadingService.GetReadings("unknown-id"));
        }

        [Fact]
        public void GivenMeterReadingThatExistsShouldReturnMeterReadings()
        {
            meterReadingService.StoreReadings(SMART_METER_ID, new List<ElectricityReading>() {
                new ElectricityReading() { Time = DateTime.Now, Reading = 25m }
            });

            var electricityReadings = meterReadingService.GetReadings(SMART_METER_ID);

            Assert.Equal(6, electricityReadings.Count);
        }
        [Fact]
        public void GivenMeterIdThatDoesNotExistShouldReturnZeroAverageConsumption()
        {
            Assert.Equal(0, meterReadingServiceForDailyAvgConsumption.AverageConsumptionPerDay("unknown-id"));
        }
        [Fact]
        public void GivenMeterReadingsShouldReturnAverageDailyConsumption()
        {
            var averageConsumption = meterReadingServiceForDailyAvgConsumption.AverageConsumptionPerDay(SMART_METER_ID);
            Assert.Equal(51.67m, averageConsumption);
        }
    }
}
