using Events_WEB_APP.Core.Entities;
using FluentValidation;

namespace Events_WEB_APP.Application.Validators
{
    /// <summary>
    /// Валидатор для категории.
    /// </summary>
    public class CategoryValidator : AbstractValidator<Category>
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CategoryValidator"/>.
        /// </summary>
        public CategoryValidator()
        {
            // Валидация Name
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Category name is required")
                .MaximumLength(30).WithMessage("Category name cannot exceed 30 characters");
        }
    }
}
