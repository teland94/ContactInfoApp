using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
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

        public async Task<ContactModel> GetContactAsync(string phoneNumber)
        {
            var contactResult = await _httpClient.GetAsync($"?phoneNumber={phoneNumber}");
            if (!contactResult.IsSuccessStatusCode)
            {
                await HandleErrorAsync(contactResult);
            }

            return await contactResult.Content.ReadFromJsonAsync<ContactModel>();
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
