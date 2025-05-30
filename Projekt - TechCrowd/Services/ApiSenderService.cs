using Projekt___TechCrowd.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Projekt_TechCrowd.Services
{
    public class ApiSenderService
    {
        private readonly HttpClient _httpClient;

        public ApiSenderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendArticleAsync(Articles article)
        {
            var json = JsonSerializer.Serialize(article);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:5069/api/Article", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Chyba: {response.StatusCode}");
            }
        }
    }
}
