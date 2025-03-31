using System;
using System.Collections.Generic;
using JOIEnergy.Services;
using JOIEnergy.Domain;
using Xunit;

namespace JOIEnergy.Tests
{
    public class MeterReadingServiceTest
    {
        private static string SMART_METER_ID = "smart-meter-id";

        private MeterReadingService meterReadingService;

        public MeterReadingServiceTest()
        {
            meterReadingService = new MeterReadingService(new Dictionary<string, List<ElectricityReading>>());

            meterReadingService.StoreReadings(SMART_METER_ID, new List<ElectricityReading>() {
                new ElectricityReading() { Time = DateTime.Now.AddMinutes(-30), Reading = 35m },
                new ElectricityReading() { Time = DateTime.Now.AddMinutes(-15), Reading = 30m }
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

            Assert.Equal(3, electricityReadings.Count);
        }
        [Fact]
        public void ShouldReturnLastSevenDaysMeterReading()
        {
            var time1 = TruncateToMinute(DateTime.Now.AddDays(-6));
            var time2 = TruncateToMinute(DateTime.Now.AddDays(-5));
            var time3 = TruncateToMinute(DateTime.Now.AddDays(-4));
            var time4 = TruncateToMinute(DateTime.Now.AddDays(-14));

            meterReadingService.StoreReadings(SMART_METER_ID, new List<ElectricityReading>
            {
                new ElectricityReading(){ Time = time1, Reading=100m},
                new ElectricityReading(){ Time = time2, Reading=200m},
                new ElectricityReading(){ Time = time3, Reading=300m},
                new ElectricityReading(){ Time = time4, Reading=400m},
            });

            var electricityReadings = meterReadingService.GetLastSevenDaysReading(SMART_METER_ID);

            Assert.Equal(3, electricityReadings.Count);
        }

        private DateTime TruncateToMinute(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
        }

    }
}
