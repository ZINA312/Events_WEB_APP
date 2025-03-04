using System.ComponentModel.DataAnnotations;

namespace Events_WEB_APP.Persistence.Contracts.Category
{
    public record CreateCategoryRequest(
    [Required][StringLength(30, MinimumLength = 1)] string Name);
}
