namespace Events_WEB_APP.Persistence.Contracts.User
{
    public record UserResponse(
    Guid Id,
    string UserName,
    string Email,
    Guid RoleId);
}
