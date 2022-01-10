using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using GetContactAPI.Models;

namespace GetContactAPI
{
    public class GetContact
    {
        private readonly Topic _topic;
        private readonly Regex _phoneRegex;

        private const string DefaultCountryCode = "UA";

        public GetContact(Data data)
        {
            _topic = new Topic(data);
            _phoneRegex = new("\\+?\\d{10,11}", RegexOptions.Compiled);
        }

        /// <summary>
        /// Возвращает основную информацию по номеру телефону
        /// </summary>
        public Task<ApiResponse<SearchResult>> GetByPhoneAsync(string phone, string countryCode = DefaultCountryCode, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(phone) || !_phoneRegex.IsMatch(phone)) throw new ArgumentException("Телефон заполнен неправильно");
            return _topic.CreateTopicAsync<SearchResult>("https://pbssrv-centralevents.com/v2.8/search", "search", phone, countryCode, cancellationToken);
        }

        /// <summary>
        /// Возвращает список тегов
        /// </summary>
        public Task<ApiResponse<DetailsResult>> GetTagsAsync(string phone, string countryCode = DefaultCountryCode, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(phone) || !_phoneRegex.IsMatch(phone)) throw new ArgumentException("Телефон заполнен неправильно");
            return _topic.CreateTopicAsync<DetailsResult>("https://pbssrv-centralevents.com/v2.8/number-detail", "detail", phone, countryCode, cancellationToken);
        }

        /// <summary>
        /// Возвращает список комментариев
        /// </summary>
        public Task<ApiResponse<CommentsResult>> GetCommentsAsync(string phone, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(phone) || !_phoneRegex.IsMatch(phone)) throw new ArgumentException("Телефон заполнен неправильно");
            return _topic.CreateTopicAsync<CommentsResult>("https://pbssrv-centralevents.com/v2.8/comments", "comments", phone, cancellationToken: cancellationToken);
        }

        public async Task SendValidationCodeAsync(string validationCode, CancellationToken cancellationToken = default)
        {
            await _topic.CreateValidationTopicAsync("https://pbssrv-centralevents.com/v2.8/verify-code", validationCode, cancellationToken);
        }
    }
}
