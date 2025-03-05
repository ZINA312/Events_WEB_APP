using Events_WEB_APP.Core.Entities;
using FluentValidation;

namespace Events_WEB_APP.Application.Validators
{
    /// <summary>
    /// Валидатор для событий.
    /// </summary>
    public class EventValidator : AbstractValidator<Event>
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EventValidator"/>.
        /// </summary>
        public EventValidator()
        {
            // Валидация Name
            RuleFor(e => e.Name)
                .NotEmpty().WithMessage("Event name is required")
                .MaximumLength(100).WithMessage("Event name cannot exceed 100 characters");

            // Валидация Description
            RuleFor(e => e.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

            // Валидация Date
            RuleFor(e => e.Date)
                .NotEmpty().WithMessage("Event date is required")
                .GreaterThanOrEqualTo(DateTime.UtcNow).WithMessage("Event date must not be in the past");

            // Валидация Location
            RuleFor(e => e.Location)
                .NotEmpty().WithMessage("Location is required")
                .MaximumLength(50).WithMessage("Location cannot exceed 50 characters");

            // Валидация CategoryId
            RuleFor(e => e.CategoryId)
                .NotEmpty().WithMessage("Category is required");

            // Валидация MaxNumOfParticipants
            RuleFor(e => e.MaxNumOfParticipants)
                .GreaterThan(0).WithMessage("Max number of participants must be greater than 0")
                .LessThanOrEqualTo(10000).WithMessage("Max number of participants cannot exceed 10,000");

            // Валидация ImagePath
            RuleFor(e => e.ImagePath)
                .MaximumLength(50).WithMessage("Image path cannot exceed 50 characters");
        }
    }
}