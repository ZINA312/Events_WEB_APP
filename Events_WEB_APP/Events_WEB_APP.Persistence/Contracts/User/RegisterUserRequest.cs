using System.ComponentModel.DataAnnotations;

namespace Events_WEB_APP.Persistence.Contracts.User
{
    public record RegisterUserRequest(
    [Required][StringLength(50)] string UserName,
    [Required][EmailAddress] string Email,
    [Required][StringLength(100, MinimumLength = 6)] string Password);
}
