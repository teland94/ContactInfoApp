using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GetContactAPI.Models
{
    public class CommentsResult
    {
        [JsonPropertyName("isAll")]
        public bool IsAll { get; set; }

        [JsonPropertyName("commentCount")]
        public int CommentCount { get; set; }

        [JsonPropertyName("deletedCount")]
        public int DeletedCount { get; set; }

        [JsonPropertyName("isCommentable")]
        public bool IsCommentable { get; set; }

        [JsonPropertyName("comments")]
        public IList<Comment> Comments { get; set; }
    }
}
