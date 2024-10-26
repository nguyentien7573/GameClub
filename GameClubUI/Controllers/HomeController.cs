using GameClubUI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace GameClubUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
            _apiBaseUrl = configuration.GetValue<string>("ApiSettings:BaseUrl");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Club> clubs = new List<Club>();

            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/clubs");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                   
                    if (jsonData is not null)
                    {
                        clubs = JsonConvert.DeserializeObject<List<Club>>(jsonData);
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Unable to retrieve clubs from the server.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error occurred while fetching clubs: {ex.Message}";
            }

            return View(clubs);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string searchTerm = "")
        {
            List<Club> clubs = new List<Club>();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/clubs/search?search={searchTerm}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                   
                    if (jsonData is not null)
                    {
                        clubs = JsonConvert.DeserializeObject<List<Club>>(jsonData);
                    }
                }
            }
            else
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/clubs");
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();

                    if (jsonData is not null) 
                    {
                        clubs = JsonConvert.DeserializeObject<List<Club>>(jsonData);
                    }
                }
            }

            return View(clubs);
        }
    }
}
