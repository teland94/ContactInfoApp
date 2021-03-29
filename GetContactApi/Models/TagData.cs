using System.Text.Json.Serialization;

namespace GetContactAPI.Models
{
    public class TagData
    {
        /// <summary>
        /// Тег
        /// </summary>
        [JsonPropertyName("tag")]
        public string Tag { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("isNew")]
        public bool IsNew { get; set; }

        [JsonPropertyName("removable")]
        public bool Removable { get; set; }

        [JsonPropertyName("askReason")]
        public bool AskReason { get; set; }
    }
}