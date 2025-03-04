using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.UnitsOfWork;
using Npgsql.EntityFrameworkCore.PostgreSQL.ValueGeneration.Internal;

namespace Events_WEB_APP.Application.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            ValidateCategory(category);
            await CheckUniqueCategoryName(category.Name);

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.CommitAsync();
            return category;
        }

        public async Task<Category> DeleteCategoryAsync(Guid categoryId)
        {
            var category = await ValidateCategoryExists(categoryId);
            await _unitOfWork.Categories.DeleteAsync(category);
            await _unitOfWork.CommitAsync();
            return category;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return (await _unitOfWork.Categories.GetAllAsync()).ToList();
        }

        public async Task<Category> GetCategoryByIdAsync(Guid categoryId)
        {
            return await ValidateCategoryExists(categoryId);
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            ValidateCategory(category);
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

        private void ValidateCategory(Category category)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(category.Name))
                errors.Add("Category name is required");

            if (category.Name?.Length > 30)
                errors.Add("Category name cannot exceed 30 characters");

            if (errors.Any())
                throw new ArgumentException(string.Join(", ", errors));
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