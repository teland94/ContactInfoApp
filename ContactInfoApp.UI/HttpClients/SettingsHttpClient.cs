using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ContactInfoApp.Shared.Models;

namespace ContactInfoApp.UI.HttpClients
{
    public class SettingsHttpClient
    {
        private readonly HttpClient _httpClient;

        private static UiSettingsModel _uiSettings;

        public SettingsHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UiSettingsModel> GetUiSettingsAsync()
        {
            return _uiSettings ??= await _httpClient.GetFromJsonAsync<UiSettingsModel>("GetUiSettings");
        }
    }
}
