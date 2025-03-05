using Events_WEB_APP.Core.Entities;
using FluentValidation;

namespace Events_WEB_APP.Application.Validators
{
    /// <summary>
    /// Валидатор для пользователей.
    /// </summary>
    public class UserValidator : AbstractValidator<User>
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="UserValidator"/>.
        /// </summary>
        public UserValidator()
        {
            // Валидация UserName
            RuleFor(u => u.UserName)
                .NotEmpty().WithMessage("User name is required")
                .MaximumLength(20).WithMessage("User name cannot exceed 20 characters");

            // Валидация Email
            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(254).WithMessage("Email cannot exceed 254 characters");

            // Валидация PasswordHash
            RuleFor(u => u.PasswordHash)
                .NotEmpty().WithMessage("Password is required");

            // Валидация RoleId
            RuleFor(u => u.RoleId)
                .NotEmpty().WithMessage("Role is required");
        }
    }
}