using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjeStaj.Models;

namespace ProjeStaj.Controllers
{
    public class RapidApiController : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<ApiMovieViewModel> apiMovieViewModels = new List<ApiMovieViewModel>();
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://imdb-top-100-movies.p.rapidapi.com/"),
                Headers =
    {
        { "x-rapidapi-key", "e7ef1d0a7emsh35a4b39a678262fp1ec392jsnfea9293e2c5a" },
        { "x-rapidapi-host", "imdb-top-100-movies.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                apiMovieViewModels=JsonConvert.DeserializeObject<List<ApiMovieViewModel>>(body);
                return View(apiMovieViewModels);
            }

        }

        public async Task<IActionResult> Exchange()
        {
             var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://booking-com.p.rapidapi.com/v1/metadata/exchange-rates?currency=TRY&locale=en-gb"),
                Headers =
    {
        { "x-rapidapi-key", "e7ef1d0a7emsh35a4b39a678262fp1ec392jsnfea9293e2c5a" },
        { "x-rapidapi-host", "booking-com.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var values  = JsonConvert.DeserializeObject<ExchangeViewModel>(body);
                return View(values.exchange_rates.ToList());
            }
        }
    }
}
