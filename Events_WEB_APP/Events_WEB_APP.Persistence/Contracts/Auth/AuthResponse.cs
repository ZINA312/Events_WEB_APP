namespace Events_WEB_APP.Persistence.Contracts.Auth
{
    /// <summary>
    /// Представляет ответ аутентификации, содержащий токены доступа и обновления.
    /// </summary>
    /// <param name="AccessToken">Токен доступа.</param>
    /// <param name="RefreshToken">Токен обновления.</param>
    public record AuthResponse(string AccessToken, string RefreshToken);
}
