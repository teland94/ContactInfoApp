using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GetContactAPI.Models
{
    public class Comment
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("authorImage")]
        public string AuthorImage { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }

        [JsonPropertyName("vote")]
        public int Vote { get; set; }

        [JsonPropertyName("liked")]
        public int Liked { get; set; }

        [JsonPropertyName("disliked")]
        public int Disliked { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("deletable")]
        public bool Deletable { get; set; }

        [JsonPropertyName("reportable")]
        public bool Reportable { get; set; }
    }
}
