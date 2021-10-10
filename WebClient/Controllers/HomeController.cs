using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using WebClient.Diagnotics;
using WebClient.Models;

namespace WebClient.Controllers
{
    public class HomeController : Controller
    {
        private WebClientDiagnostics _diagnostics;
        private IHttpClientFactory _httpClientFactory;
        public HomeController(WebClientDiagnostics diagnostics,IHttpClientFactory httpClientFactoryu)
        {
            _diagnostics = diagnostics;
            _httpClientFactory = httpClientFactoryu;
        }

        public async Task<IActionResult> Index()
        {
            using (var activity = _diagnostics.OnHome("with extra data"))
            {
                activity?.AddTag("tag1", "value");
                activity?.AddEvent(new ActivityEvent("my event"));
                activity?.AddBaggage("bag1", "value bag1");

                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync("http://localhost:33018/WeatherForecast");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
