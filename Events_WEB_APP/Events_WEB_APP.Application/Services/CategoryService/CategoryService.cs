using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.UnitsOfWork;
using FluentValidation;

namespace Events_WEB_APP.Application.Services.CategoryService
{
    /// <summary>
    /// Сервис для управления категориями.
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<Category> _validator;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CategoryService"/>.
        /// </summary>
        /// <param name="unitOfWork">Единица работы для доступа к репозиториям.</param>
        /// <param name="validator">Валидатор для проверки категорий.</param>
        public CategoryService(IUnitOfWork unitOfWork, IValidator<Category> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        /// <summary>
        /// Создает новую категорию.
        /// </summary>
        /// <param name="category">Категория для создания.</param>
        /// <returns>Созданная категория.</returns>
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            await ValidateCategoryAsync(category);
            await CheckUniqueCategoryName(category.Name);

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.CommitAsync();
            return category;
        }

        /// <summary>
        /// Удаляет категорию по идентификатору.
        /// </summary>
        /// <param name="categoryId">Идентификатор категории для удаления.</param>
        /// <returns>Удаленная категория.</returns>
        public async Task<Category> DeleteCategoryAsync(Guid categoryId)
        {
            var category = await ValidateCategoryExists(categoryId);
            await _unitOfWork.Categories.DeleteAsync(category);
            await _unitOfWork.CommitAsync();
            return category;
        }

        /// <summary>
        /// Получает все категории.
        /// </summary>
        /// <returns>Список категорий.</returns>
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return (await _unitOfWork.Categories.GetAllAsync()).ToList();
        }

        /// <summary>
        /// Получает категорию по идентификатору.
        /// </summary>
        /// <param name="categoryId">Идентификатор категории.</param>
        /// <returns>Найдена категория.</returns>
        public async Task<Category> GetCategoryByIdAsync(Guid categoryId)
        {
            return await ValidateCategoryExists(categoryId);
        }

        /// <summary>
        /// Обновляет существующую категорию.
        /// </summary>
        /// <param name="category">Обновленная категория.</param>
        /// <returns>Обновленная категория.</returns>
        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            await ValidateCategoryAsync(category);
            var existingCategory = await ValidateCategoryExists(category.Id);
            await CheckUniqueCategoryName(category.Name, existingCategory.Id);

            existingCategory.Name = category.Name;
            await _unitOfWork.Categories.UpdateAsync(existingCategory);
            await _unitOfWork.CommitAsync();
            return existingCategory;
        }

        private async Task<Category> ValidateCategoryExists(Guid categoryId)
        {
            return await _unitOfWork.Categories.GetByIdAsync(categoryId)
                ?? throw new KeyNotFoundException($"Category with ID {categoryId} not found");
        }

        private async Task ValidateCategoryAsync(Category category)
        {
            var validationResult = await _validator.ValidateAsync(category);

            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException(errors);
            }
        }

        private async Task CheckUniqueCategoryName(string name, Guid? excludedId = null)
        {
            var existing = await _unitOfWork.Categories.GetWhereAsync(c =>
                c.Name == name &&
                (!excludedId.HasValue || c.Id != excludedId.Value));

            if (existing.Any())
                throw new InvalidOperationException($"Category name '{name}' already exists");
        }
    }
}