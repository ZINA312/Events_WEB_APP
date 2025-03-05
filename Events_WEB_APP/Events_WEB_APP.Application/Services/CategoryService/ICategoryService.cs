using Events_WEB_APP.Core.Entities;

namespace Events_WEB_APP.Application.Services.CategoryService
{
    /// <summary>
    /// Интерфейс для сервиса управления категориями.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Создает новую категорию.
        /// </summary>
        /// <param name="category">Категория для создания.</param>
        /// <returns>Созданная категория.</returns>
        Task<Category> CreateCategoryAsync(Category category);
        /// <summary>
        /// Удаляет категорию по идентификатору.
        /// </summary>
        /// <param name="categoryId">Идентификатор категории для удаления.</param>
        /// <returns>Удаленная категория.</returns>
        Task<Category> DeleteCategoryAsync(Guid categoryId);
        /// <summary>
        /// Получает все категории.
        /// </summary>
        /// <returns>Список категорий.</returns>
        Task<List<Category>> GetAllCategoriesAsync();
        /// <summary>
        /// Получает категорию по идентификатору.
        /// </summary>
        /// <param name="categoryId">Идентификатор категории.</param>
        /// <returns>Найдена категория.</returns>
        Task<Category> GetCategoryByIdAsync(Guid categoryId);
        /// <summary>
        /// Обновляет существующую категорию.
        /// </summary>
        /// <param name="category">Обновленная категория.</param>
        /// <returns>Обновленная категория.</returns>
        Task<Category> UpdateCategoryAsync(Category category);
    }
}
