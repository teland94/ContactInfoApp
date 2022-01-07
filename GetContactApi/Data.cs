using System;

namespace GetContactAPI
{
    /// <summary>
    /// Главные данные для работы API
    /// </summary>
    public class Data
    {
        /// <summary>
        /// Токен из конфиг-файла
        /// </summary>
        public string Token { get; }

        /// <summary>
        /// Зашифрованный ключ из конфиг-файла
        /// </summary>
        /// <remarks>
        /// Длина - 64 символа
        /// </remarks>
        public string AesKey { get; }

        /// <summary>
        /// Ключ шифрования
        /// </summary>
        public string Key { get; } = "y1gY|J%&6V kTi$>_Ali8]/xCqmMMP1$*)I8FwJ,*r_YUM 4h?@7+@#<>+w-e3VW";

        public Data(string token, string aesKey)
        {
            Token = token;
            AesKey = aesKey;
            Validate();
        }

        public Data(string token, string aesKey, string key)
        {
            Token = token;
            AesKey = aesKey;
            Key = key;
            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Token))
                throw new ArgumentNullException("Token is empty or invalid");

            if (string.IsNullOrWhiteSpace(AesKey) || AesKey.Length != 64)
                throw new ArgumentNullException("AES key is empty or invalid");
        }
    }
}
