namespace Events_WEB_APP.Infrastructure.JWT
{
    /// <summary>
    /// Опции для настройки JWT (JSON Web Tokens).
    /// </summary>
    public class JWTOptions
    {
        /// <summary>
        /// Секретный ключ для подписи токенов.
        /// </summary>
        public string SecretKey { get; set; } = string.Empty;

        /// <summary>
        /// Время действия токена в часах.
        /// </summary>
        public int ExpiresHours { get; set; }
    }
}
