using System;
using System.Text.Json.Serialization;

namespace GetContactAPI.Models
{
    public class SubscriptionInfo
    {
        /// <summary>
        /// Лимиты
        /// </summary>
        [JsonPropertyName("usage")]
        public SubscriptionUsage Usage { get; set; }

        [JsonPropertyName("isPro")]
        public bool IsPro { get; set; }

        [JsonPropertyName("isTrialUsed")]
        public bool IsTrialUsed { get; set; }

        [JsonPropertyName("premiumType")]
        public string PremiumType { get; set; }

        [JsonPropertyName("premiumTypeName")]
        public string PremiumTypeName { get; set; }

        [JsonPropertyName("receiptEndDate")]
        public DateTime ReceiptEndDate { get; set; }

        [JsonPropertyName("receiptStartDate")]
        public DateTime ReceiptStartDate { get; set; }

        [JsonPropertyName("renewDate")]
        public DateTime RenewDate { get; set; }
    }
}