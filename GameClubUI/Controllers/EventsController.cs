using GameClubUI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace GameClubUI.Controllers
{
    public class EventsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public EventsController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _apiBaseUrl = configuration.GetValue<string>("ApiSettings:BaseUrl");
        }

        [HttpGet]
        public async Task<IActionResult> CreateEvent(int clubId)
        {
            var clubResponse = await _httpClient.GetAsync($"{_apiBaseUrl}/api/clubs/{clubId}");
            if (clubResponse.IsSuccessStatusCode)
            {
                var clubJson = await clubResponse.Content.ReadAsStringAsync();
                var club = JsonConvert.DeserializeObject<Club>(clubJson);
                ViewBag.ClubName = club.Name;
            }
            else
            {
                ViewBag.ClubName = "Unknown Club";
            }

            return View(new Event());
        }

        [HttpPost("/Events/CreateEvent/{clubId}")]
        public async Task<IActionResult> CreateEvent([FromRoute] int clubId, Event gameEvent)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please provide valid data for all fields.";
                return View(gameEvent);
            }

            gameEvent.ClubId = clubId;

            var jsonData = JsonConvert.SerializeObject(gameEvent);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/api/clubs/{clubId}/events", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ClubEvents", "Clubs",new { clubId = clubId });
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                TempData["Error"] = responseContent;
                return View(gameEvent);
            }
        }
    }
}
