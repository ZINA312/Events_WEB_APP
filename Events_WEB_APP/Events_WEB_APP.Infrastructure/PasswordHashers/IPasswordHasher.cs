namespace Events_WEB_APP.Infrastructure.PasswordHashers
{
    /// <summary>
    /// Интерфейс для работы с хешированием паролей.
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// Генерирует хеш пароля.
        /// </summary>
        /// <param name="password">Пароль, который необходимо захешировать.</param>
        /// <returns>Хешированный пароль.</returns>
        string Generate(string password);

        /// <summary>
        /// Проверяет, соответствует ли заданный пароль хешированному паролю.
        /// </summary>
        /// <param name="password">Пароль, который необходимо проверить.</param>
        /// <param name="hashedPassword">Хешированный пароль для проверки.</param>
        /// <returns><c>true</c>, если пароль соответствует; в противном случае <c>false</c>.</returns>
        public bool Verify(string password, string hashedPassword);
    }
}