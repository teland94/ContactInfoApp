using System;
using System.Dynamic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GetContactAPI.Converters;
using GetContactAPI.Exceptions;
using GetContactAPI.Models;

namespace GetContactAPI
{
    internal sealed class Topic
    {
        private readonly Data _data;
        private readonly JsonSerializerOptions _jsonOptions;

        public Topic(Data data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _jsonOptions = new()
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                Converters =
                {
                    new GetContactDateTimeConverter()
                }
            };
        }

        public async Task CreateValidationTopicAsync(string url, string validationCode, CancellationToken ct = default)
        {
            var reqObj = new
            {
                token = _data.Token,
                validationCode
            };

            await SendRequestAsync<object>(url, reqObj, ct);
        }

        /// <summary>
        /// Формирование зашифрованного запроса
        /// </summary>
        /// <returns>Получение дешифрованного запроса в формате json</returns>
        public async Task<ApiResponse<T>> CreateTopicAsync<T>(string url, string source, string phone, string countryCode = null, CancellationToken cancellationToken = default)
        {
            dynamic reqObj = new ExpandoObject();

            reqObj.phoneNumber = phone;
            reqObj.source = source;
            reqObj.token = _data.Token;

            if (countryCode != null)
            {
                reqObj.countryCode = countryCode;
            }

            return await SendRequestAsync<T>(url, reqObj, cancellationToken);
        }

        private Task<ApiResponse<T>> SendRequestAsync<T>(string url, object reqObj, CancellationToken cancellationToken = default)
        {
            string timestamp = ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString();

            string req = JsonSerializer.Serialize(reqObj, options: _jsonOptions);
            string signature = Cryptography.EncryptToSHA256(timestamp.Replace("\r\n", "") + "-" + req, _data.Key); // signature
            string crypt = Cryptography.EncryptAes256ECB(req, _data.AesKey);

            return SendDataAsync<T>(url, "{\"data\":\"" + crypt + "\"}", timestamp, signature, cancellationToken);
        }

        /// <summary>
        /// Отправка готового запроса на сервер, с последующей дешифровкой
        /// </summary>
        private async Task<ApiResponse<T>> SendDataAsync<T>(string url, string data, string timestamp, string signature, CancellationToken cancellationToken = default)
        {
            using HttpClient client = new();
            using HttpRequestMessage request = new(HttpMethod.Post, url);

            request.Content = new StringContent(data, Encoding.UTF8, "application/json");

            request.Headers.Add("X-App-Version", "5.6.2");
            request.Headers.Add("X-Token", _data.Token);
            request.Headers.Add("X-Os", "android 5.0");
            request.Headers.Add("X-Client-Device-Id", "14130e29cebe9c39");
            request.Headers.Add("Accept-Encoding", "deflate");
            request.Headers.Add("X-Req-Timestamp", timestamp);
            request.Headers.Add("X-Req-Signature", signature);
            request.Headers.Add("X-Encrypted", "1");

            using HttpResponseMessage response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false);

            using Stream content = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
            using JsonDocument rawResponse = await JsonDocument.ParseAsync(content, cancellationToken: cancellationToken).ConfigureAwait(false);

            if (!rawResponse.RootElement.TryGetProperty("data", out JsonElement rawData))
                throw new ApplicationException("Failed to get \"data\" from response!");

            string decryptedResponse = Cryptography.DecryptAes256ECB(rawData.ToString(), _data.AesKey);

            if (!response.IsSuccessStatusCode)
            {
                var errorResult = JsonSerializer.Deserialize<ApiResponse<ErrorResult>>(decryptedResponse, _jsonOptions);
                throw new GetContactRequestException(response.ReasonPhrase, response.StatusCode, errorResult);
            }

            return JsonSerializer.Deserialize<ApiResponse<T>>(decryptedResponse, _jsonOptions);
        }
    }
}
