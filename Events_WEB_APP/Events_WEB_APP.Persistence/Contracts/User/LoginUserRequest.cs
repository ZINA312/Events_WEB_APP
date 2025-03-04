using System.ComponentModel.DataAnnotations;

namespace Events_WEB_APP.Persistence.Contracts.User
{
    public record LoginUserRequest(
    [Required][EmailAddress] string Email,
    [Required] string Password);
}
