using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GetContactAPI.Models
{
    public class DetailsResult
    {
        /// <summary>
        /// Список тегов
        /// </summary>
        [JsonPropertyName("tags")]
        public IList<TagData> Tags { get; set; }

        /// <summary>
        /// Удалённые теги (доступно для премиума)
        /// </summary>
        [JsonPropertyName("deletedTags")]
        public IList<string> DeletedTags { get; set; }

        /// <summary>
        /// Количество удалённых тегов (доступно для премиума)
        /// </summary>
        [JsonPropertyName("deletedTagCount")]
        public int DeletedTagCount { get; set; }
    }
}