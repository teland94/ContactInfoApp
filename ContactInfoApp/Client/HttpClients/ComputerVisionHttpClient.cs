using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ContactInfoApp.Shared;

namespace ContactInfoApp.Client.HttpClients
{
    public class ComputerVisionHttpClient
    {
        private readonly HttpClient _httpClient;

        public ComputerVisionHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetText(string image)
        {
            var res = await _httpClient.PostAsJsonAsync("ocr", new OcrRequestModel
            {
                Image = image
            });

            res.EnsureSuccessStatusCode();

            return await res.Content.ReadAsStringAsync();
        }
    }
}
