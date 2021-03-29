using System.Text.Json.Serialization;

namespace GetContactAPI.Models
{
    public class SubscriptionUsageInfo
    {
        /// <summary>
        /// Количество оставшихся запросов
        /// </summary>
        [JsonPropertyName("remainingCount")]
        public int RemainingCount { get; set; }

        /// <summary>
        /// Максимум запросов
        /// </summary>
        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonPropertyName("isColorRed")]
        public bool IsColorRed { get; set; }

        [JsonPropertyName("showOffer")]
        public bool ShowOffer { get; set; }

        [JsonPropertyName("showPackages")]
        public bool ShowPackages { get; set; }
    }
}