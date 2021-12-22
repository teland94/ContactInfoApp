using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ContactInfoApp.Shared.Models;
using ContactInfoApp.Shared.Request;

namespace ContactInfoApp.UI.HttpClients
{
    public class SearchContactHistoryHttpClient
    {
        private readonly HttpClient _httpClient;

        public SearchContactHistoryHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<IEnumerable<SearchContactHistoryModel>> GetAsync()
        {
            return _httpClient.GetFromJsonAsync<IEnumerable<SearchContactHistoryModel>>("");
        }

        public async Task<IEnumerable<ContactHistoryPhoneNumberInfoModel>> GetPhoneNumbersInfoAsync(ContactHistoryPhoneNumberInfoRequestModel model)
        {
            var res = await _httpClient.PostAsJsonAsync("GetPhoneNumbersInfo", model);

            res.EnsureSuccessStatusCode();

            return await res.Content.ReadFromJsonAsync<IEnumerable<ContactHistoryPhoneNumberInfoModel>>();
        }

        public Task DeleteAsync(int id)
        {
            return _httpClient.DeleteAsync($"{id}");
        }
    }
}
