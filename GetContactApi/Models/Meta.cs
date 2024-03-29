﻿using System.Text.Json.Serialization;

namespace GetContactAPI.Models
{
    public class Meta
    {
        /// <summary>
        /// Код ошибки, если всё ок, должно быть null
        /// </summary>
        [JsonPropertyName("errorCode")]
        public string ErrorCode { get; set; }

        /// <summary>
        /// Сообщение об ошибке, если всё ок, должно быть null
        /// </summary>
        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Хранит в себе тоже самое что и HTTP код ответа
        /// </summary>
        [JsonPropertyName("httpStatusCode")]
        public int HttpStatusCode { get; set; }

        /// <summary>
        /// ID запроса
        /// </summary>
        [JsonPropertyName("requestId")]
        public string RequestId { get; set; }

        /// <summary>
        /// Произошла ошибка или нет
        /// </summary>
        [JsonIgnore]
        public bool IsRequestError => HttpStatusCode != 200 || !string.IsNullOrEmpty(ErrorCode) || !string.IsNullOrEmpty(ErrorMessage);
    }
}