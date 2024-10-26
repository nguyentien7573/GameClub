using GameClubUI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace GameClubUI.Controllers
{
    public class ClubsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        public ClubsController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _apiBaseUrl = configuration.GetValue<string>("ApiSettings:BaseUrl");
        }

        [HttpGet]
        public IActionResult CreateClub()
        {
            return View(new Club());
        }

        [HttpPost]
        public async Task<IActionResult> CreateClub(Club club)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please provide valid data for all fields.";
                return View(club);
            }

            var existingClubsResponse = await _httpClient.GetAsync($"{_apiBaseUrl}/api/clubs");
            var existingClubsJson = await existingClubsResponse.Content.ReadAsStringAsync();
            var existingClubs = JsonConvert.DeserializeObject<List<Club>>(existingClubsJson);

            if (existingClubs.Any(c => c.Name == club.Name))
            {
                TempData["Error"] = "A club with this name already exists.";
                return View(club);
            }

            var jsonData = JsonConvert.SerializeObject(club);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/api/clubs", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Error"] = "Failed to create club. Please try again.";
                return View(club);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ClubEvents(int clubId)
        {
            if (clubId == 0)
            {
                TempData["Error"] = "Invalid Club ID.";
                return RedirectToAction("Index", "Home");
            }


            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/clubs/{clubId}/events");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMessage = "Unable to retrieve events.";
                return View(new List<Event>());
            }

            var jsonData = await response.Content.ReadAsStringAsync();
            var events = JsonConvert.DeserializeObject<List<Event>>(jsonData);

            return View(events);
        }
    }
}
