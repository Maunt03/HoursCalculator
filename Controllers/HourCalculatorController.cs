using Microsoft.AspNetCore.Mvc;
using HoursCalculator;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace HoursCalculator.Controllers
{
    public class HourCalculatorController : Controller
    {
        [HttpGet]
        [Route("HourCalculator/Index")]
        public IActionResult Index()
        {
            DateTime startDate;
            DateTime endDate;

            try
            {
                startDate = DateTime.ParseExact(HttpContext.Request.Query["startDate"], "dd.M.yyyy", null);
                endDate = DateTime.ParseExact(HttpContext.Request.Query["endDate"], "dd.M.yyyy", null);
            }
            catch
            {
                return StatusCode(400);
            }
            if (startDate > endDate) return StatusCode(400);

            Events events = GoogleCalendarAPI.CalculateHours(startDate, endDate);
            if (events == null) return StatusCode(500);
            foreach (var item in events.Items)
            {
                /*to do*/
            }

            return View();
        }
    }
}
