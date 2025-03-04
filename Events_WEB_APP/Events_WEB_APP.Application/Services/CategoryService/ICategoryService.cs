using Events_WEB_APP.Core.Entities;

namespace Events_WEB_APP.Application.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<Category> CreateCategoryAsync(Category category);
        Task<Category> DeleteCategoryAsync(Guid categoryId);
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(Guid categoryId);
        Task<Category> UpdateCategoryAsync(Category category);
    }
}
