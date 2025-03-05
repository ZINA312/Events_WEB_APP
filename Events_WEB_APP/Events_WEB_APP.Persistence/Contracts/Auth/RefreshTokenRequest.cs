namespace Events_WEB_APP.Persistence.Contracts.Auth
{
    /// <summary>
    /// Представляет запрос на обновление токена, содержащий токен доступа и токен обновления.
    /// </summary>
    /// <param name="AccessToken">Токен доступа.</param>
    /// <param name="RefreshToken">Токен обновления.</param>
    public record RefreshTokenRequest(string AccessToken, string RefreshToken);
}
