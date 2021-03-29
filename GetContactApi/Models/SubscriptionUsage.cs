using System.Text.Json.Serialization;

namespace GetContactAPI.Models
{
    public class SubscriptionUsage
    {
        /// <summary>
        /// Лимиты на поиск
        /// </summary>
        [JsonPropertyName("search")]
        public SubscriptionUsageInfo Search { get; set; }

        /// <summary>
        /// Лимиты на теги/детали о номере
        /// </summary>
        [JsonPropertyName("numberDetail")]
        public SubscriptionUsageInfo NumberDetail { get; set; }

        [JsonPropertyName("callBenefits")]
        public SubscriptionUsageInfo CallBenefits { get; set; }

        [JsonPropertyName("dailySearch")]
        public SubscriptionUsageInfo DailySearch { get; set; }

        [JsonPropertyName("share")]
        public SubscriptionUsageInfo Share { get; set; }

        [JsonPropertyName("trustScore")]
        public SubscriptionUsageInfo TrustScore { get; set; }
    }
}