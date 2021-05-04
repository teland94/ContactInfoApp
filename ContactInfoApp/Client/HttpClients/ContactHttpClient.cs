using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using ContactInfoApp.Client.Exceptions;
using ContactInfoApp.Shared.Models;

namespace ContactInfoApp.Client.HttpClients
{
    public class ContactHttpClient
    {
        private readonly HttpClient _httpClient;

        public ContactHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ContactModel> SearchContactAsync(string phoneNumber)
        {
            var contactResult = await _httpClient.GetAsync($"Search?phoneNumber={phoneNumber}");
            if (!contactResult.IsSuccessStatusCode)
            {
                await HandleErrorAsync(contactResult);
            }

            return await contactResult.Content.ReadFromJsonAsync<ContactModel>();
        }

        public async Task<NumberDetailModel> GetNumberDetailAsync(string phoneNumber, int? contactId = null)
        {
            var builder = new UriBuilder($"{_httpClient.BaseAddress}NumberDetail");
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["phoneNumber"] = phoneNumber;

            if (contactId != null)
            {
                query["contactId"] = contactId.Value.ToString();
            }

            builder.Query = query.ToString() ?? string.Empty;

            var numberDetailResult = await _httpClient.GetAsync(builder.Uri);

            if (!numberDetailResult.IsSuccessStatusCode)
            {
                await HandleErrorAsync(numberDetailResult);
            }

            return await numberDetailResult.Content.ReadFromJsonAsync<NumberDetailModel>();
        }

        public async Task<bool> VerifyCodeAsync(string validationCode)
        {
            var verifyCodeResult = await _httpClient.GetAsync($"VerifyCode?validationCode={validationCode}");
            if (!verifyCodeResult.IsSuccessStatusCode)
            {
                await HandleErrorAsync(verifyCodeResult);
            }

            return verifyCodeResult.IsSuccessStatusCode;
        }

        private async Task HandleErrorAsync(HttpResponseMessage result)
        {
            if (result.StatusCode == HttpStatusCode.Forbidden)
            {
                var errorResultString = await result.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(errorResultString))
                {
                    throw new ContactRequestException(result.StatusCode, result.ReasonPhrase);
                }

                var errorResult = JsonSerializer.Deserialize<ErrorResult>(errorResultString);
                throw new ContactRequestException(result.StatusCode, result.ReasonPhrase, errorResult);
            }
            
            throw new ContactRequestException(result.StatusCode, result.ReasonPhrase);
        }
    }
}
