using System.ComponentModel.DataAnnotations;

namespace Events_WEB_APP.Persistence.Contracts.Role
{
    public record UpdateRoleRequest(
        [Required(ErrorMessage = "Role name is required")]
        [StringLength(50, ErrorMessage = "Role name cannot exceed 50 characters")]
        string Name
    );
}
