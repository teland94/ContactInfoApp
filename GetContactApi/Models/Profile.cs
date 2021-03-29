using System.Text.Json.Serialization;

namespace GetContactAPI.Models
{
    /// <summary>
    /// Основной профиль пользователя
    /// </summary>
    public class Profile
    {
        /// <summary>
        /// Имена пользователя
        /// </summary>
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Имя
        /// <para/>Лучше брать значение из <see cref="DisplayName"/>, если там ничего не будет, то смотреть тут и в <seealso cref="Surname"/>
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Фамилия
        /// <para/>Лучше брать значение из <see cref="DisplayName"/>, если там ничего не будет, то смотреть тут и в <seealso cref="Name"/>
        /// </summary>
        [JsonPropertyName("surname")]
        public string Surname { get; set; }

        /// <summary>
        /// Страна пользователя
        /// </summary>
        [JsonPropertyName("countryCode")]
        public string Country { get; set; }

        /// <summary>
        /// Количество найденных тегов
        /// </summary>
        [JsonPropertyName("tagCount")]
        public int? TagCount { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Картинка профиля, не уверен что бывает
        /// </summary>
        [JsonPropertyName("profileImage")]
        public string ProfileImage { get; set; }
    }
}