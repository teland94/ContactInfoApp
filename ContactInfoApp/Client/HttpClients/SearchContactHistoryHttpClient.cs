using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ContactInfoApp.Shared.Models;

namespace ContactInfoApp.Client.HttpClients
{
    public class SearchContactHistoryHttpClient
    {
        private readonly HttpClient _httpClient;

        public SearchContactHistoryHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<IEnumerable<SearchContactHistoryModel>> Get()
        {
            return _httpClient.GetFromJsonAsync<IEnumerable<SearchContactHistoryModel>>("");
        }
    }
}
