using Microsoft.AspNetCore.Mvc;
using HoursCalculator;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Net;

namespace HoursCalculator.Controllers
{
    public class HoursCalculatorController : Controller
    {
        [HttpPost]
        [Route("HoursCalculator/Index")]
        public IActionResult Index()
        {
            DateTime startDate;
            DateTime endDate;

            try
            {
                startDate = DateTime.ParseExact(HttpContext.Request.Form["startDate"], "dd.M.yyyy", null);
                endDate = DateTime.ParseExact(HttpContext.Request.Form["endDate"], "dd.M.yyyy", null);
            }
            catch
            {
                return StatusCode(400);
            }
            if (startDate > endDate) return StatusCode(400);

            Events events = GoogleCalendarAPI.CalculateHours(startDate, endDate);
            if (events == null) return StatusCode(500);
            Dictionary<string, int> tutors = new Dictionary<string, int>();
            foreach (var item in events.Items)
            {
                string tutorName = item.Summary.Split('(', ')')[1].Split(',')[0].Trim();
                if (tutors.ContainsKey(tutorName))
                {
                    tutors[tutorName]++;
                }
                else
                {
                    tutors.Add(tutorName, 1);
                }
            }
            foreach (var item in tutors)
            {
                Console.WriteLine(item.Key + " " + item.Value);
            }
            return View(tutors);
        }
    }
}
