using System.Text.Json.Serialization;

namespace ContactInfoApp.Shared
{
    public class ErrorResult
    {
        [JsonPropertyName("image")]
        public string Image { get; set; }
    }
}
