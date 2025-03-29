using JOIEnergy.Domain;

namespace JOIEnergy.Services;

public interface IHighUsageService
{
    ElectricityReading GetHighUsageReading(string meterId);
}