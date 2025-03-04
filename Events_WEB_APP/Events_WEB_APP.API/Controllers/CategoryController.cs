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

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }


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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(_mapper.Map<List<CategoryResponse>>(categories));
        }

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
