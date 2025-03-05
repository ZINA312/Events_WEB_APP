namespace Events_WEB_APP.Infrastructure.PasswordHashers
{
    /// <summary>
    /// Реализация интерфейса <see cref="IPasswordHasher"/> для хеширования паролей.
    /// </summary>
    public class PasswordHasher : IPasswordHasher
    {
        /// <summary>
        /// Генерирует хеш пароля.
        /// </summary>
        /// <param name="password">Пароль, который необходимо захешировать.</param>
        /// <returns>Хешированный пароль.</returns>
        public string Generate(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        }

        /// <summary>
        /// Проверяет, соответствует ли заданный пароль хешированному паролю.
        /// </summary>
        /// <param name="password">Пароль, который необходимо проверить.</param>
        /// <param name="hashedPassword">Хешированный пароль для проверки.</param>
        /// <returns><c>true</c>, если пароль соответствует; в противном случае <c>false</c>.</returns>
        public bool Verify(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
        }
    }
}
