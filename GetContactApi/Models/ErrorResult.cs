using System.Text.Json.Serialization;

namespace GetContactAPI.Models
{
    public class ErrorResult
    {
        [JsonPropertyName("image")]
        public string Image { get; set; }
    }
}
