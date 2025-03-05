using Events_WEB_APP.Application.Services.CategoryService;
using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.Contracts.Category;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Events_WEB_APP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CategoryController"/>.
        /// </summary>
        /// <param name="categoryService">Сервис для управления категориями.</param>
        /// <param name="mapper">Mapper для преобразования между сущностями и DTO.</param>
        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        /// <summary>
        /// Создает новую категорию.
        /// </summary>
        /// <param name="request">Запрос, содержащий детали категории.</param>
        /// <returns>Ответ с созданной категорией.</returns>
        [HttpPost]
        public async Task<ActionResult<CategoryResponse>> CreateCategory(
            [FromBody] CreateCategoryRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var category = _mapper.Map<Category>(request);
                var createdCategory = await _categoryService.CreateCategoryAsync(category);
                return CreatedAtAction(
                    nameof(GetCategoryById),
                    new { categoryId = createdCategory.Id },
                    _mapper.Map<CategoryResponse>(createdCategory));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Получает все категории.
        /// </summary>
        /// <returns>Список категорий.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(_mapper.Map<List<CategoryResponse>>(categories));
        }

        /// <summary>
        /// Получает категорию по ее ID.
        /// </summary>
        /// <param name="categoryId">ID категории для получения.</param>
        /// <returns>Ответ с категорией.</returns>
        [HttpGet("{categoryId}")]
        public async Task<ActionResult<CategoryResponse>> GetCategoryById(Guid categoryId)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(categoryId);
                return Ok(_mapper.Map<CategoryResponse>(category));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Обновляет существующую категорию.
        /// </summary>
        /// <param name="request">Запрос с обновленными данными категории.</param>
        /// <returns>Ответ с обновленной категорией.</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateCategory(
            [FromBody] UpdateCategoryRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var category = _mapper.Map<Category>(request);
                var updatedCategory = await _categoryService.UpdateCategoryAsync(category);
                return Ok(_mapper.Map<CategoryResponse>(updatedCategory));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Удаляет категорию по ее ID.
        /// </summary>
        /// <param name="categoryId">ID категории для удаления.</param>
        /// <returns>Результат операции удаления.</returns>
        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteCategory(Guid categoryId)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(categoryId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
