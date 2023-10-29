using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ContactInfoApp.Shared.Helpers;
using ContactInfoApp.Shared.Models;

namespace ContactInfoApp.UI.HttpClients
{
    public class SearchContactHistoryCommentHttpClient
    {
        private readonly HttpClient _httpClient;

        public SearchContactHistoryCommentHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<IEnumerable<SearchContactHistoryCommentModel>> GetBySearchContactHistoryId(int searchContactHistoryId)
        {
            var url = "GetBySearchContactHistoryId";
            var parameters = new Dictionary<string, string>
            {
                { "searchContactHistoryId", searchContactHistoryId.ToString() }
            };

            url = UrlHelpers.AddQueryParameters(url, parameters);

            return _httpClient.GetFromJsonAsync<IEnumerable<SearchContactHistoryCommentModel>>($"{url}");
        }
    }
}
