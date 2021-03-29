using System.Text.Json.Serialization;

namespace GetContactAPI.Models
{
    public class SearchResult
    {
        /// <summary>
        /// Основной профиль пользователя
        /// </summary>
        [JsonPropertyName("profile")]
        public Profile Profile { get; set; }
        /// <summary>
        /// Информация о подписке
        /// </summary>
        [JsonPropertyName("subscriptionInfo")]
        public SubscriptionInfo SubscriptionInfo { get; set; }

        [JsonPropertyName("spamInfo")]
        public SpamInfo SpamInfo { get; set; }

        [JsonPropertyName("searchedHimself")]
        public bool SearchedHimself { get; set; }

        [JsonPropertyName("limitedResult")]
        public bool LimitedResult { get; set; }
    }
}