namespace Events_WEB_APP.Persistence.Contracts.Role
{
    /// <summary>
    /// Представляет ответ для роли, содержащий информацию о роли.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор роли.</param>
    /// <param name="Name">Имя роли.</param>
    public record RoleResponse(
        Guid Id,
        string Name
    );
}
