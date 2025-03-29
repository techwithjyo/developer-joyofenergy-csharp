using JOIEnergy.Domain;
using JOIEnergy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace JOIEnergy.Controllers
{
    [Route("average-consumption")]
    public class AverageConsumptionController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        private readonly IMeterReadingService _meterReadingService;

        public AverageConsumptionController(IMeterReadingService meterReadingService)
        {
            _meterReadingService = meterReadingService;
        }
        [HttpGet("{smartMeterId}")]
        public IActionResult GetAverageDailyConsumptionNew(string smartMeterId)
        {
            var avgConsumption = _meterReadingService.AverageConsumptionPerDay(smartMeterId);
            if (avgConsumption == 0)
            {
                return NotFound($"No readings found for Smart Meter ID: {smartMeterId}");
            }
            return Ok(avgConsumption);
        }
    }
}
