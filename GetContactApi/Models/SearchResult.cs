using System;
using System.Collections.Generic;
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

        [JsonPropertyName("comments")]
        public SearchCommentsResult Comments { get; set; }
    }

    public class SearchCommentsResult
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
        public IList<SearchComment> Comments { get; set; }

        [JsonPropertyName("characterLimit")]
        public int? CharacterLimit { get; set; }
    }

    public class SearchComment
    {
        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("authorImage")]
        public string AuthorImage { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
    }
}