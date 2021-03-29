using System.Text.Json.Serialization;

namespace GetContactAPI.Models
{
    public class SpamInfo
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("degree")]
        public string Degree { get; set; }
    }
}