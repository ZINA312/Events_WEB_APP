namespace Events_WEB_APP.Persistence.Contracts.User
{
    /// <summary>
    /// Представляет ответ на запрос входа пользователя, содержащий токен аутентификации.
    /// </summary>
    /// <param name="Token">Токен аутентификации, выданный пользователю при успешном входе.</param>
    public record LoginUserResponse(string Token);
}