using Events_WEB_APP.Core.Entities;
using FluentValidation;

namespace Events_WEB_APP.Application.Validators
{
    /// <summary>
    /// Валидатор для ролей.
    /// </summary>
    public class RoleValidator : AbstractValidator<Role>
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RoleValidator"/>.
        /// </summary>
        public RoleValidator()
        {
            // Валидация Name
            RuleFor(r => r.Name)
                .NotEmpty().WithMessage("Role name is required")
                .MaximumLength(20).WithMessage("Role name cannot exceed 20 characters");
        }
    }
}