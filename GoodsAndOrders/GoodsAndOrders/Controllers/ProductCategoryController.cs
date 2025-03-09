using GoodsAndOrders.DTOs;
using GoodsAndOrders.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoodsAndOrders.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public ProductCategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Получить все категории
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        // Получить категорию по ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(Guid id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();

            return Ok(category);
        }

        // Добавить новую категорию
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] ProductCategoryDto categoryDto)
        {
            if (categoryDto == null) return BadRequest("Данные отсутствуют");

            var createdCategory = await _categoryService.CreateCategoryAsync(categoryDto);
            return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.Id }, createdCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] ProductCategoryDto categoryDto)
        {
            if (categoryDto == null) return BadRequest("Данные отсутствуют"); // Проверяем входные данные

            bool updated = await _categoryService.UpdateCategoryAsync(id, categoryDto);
            if (!updated) return NotFound(); // Если категория не найдена, возвращаем 404

            return NoContent(); // HTTP 204 (успешно, но без контента)
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            bool deleted = await _categoryService.DeleteCategoryAsync(id);
            if (!deleted) return NotFound(); // Если категории нет, возвращаем 404

            return NoContent(); // HTTP 204 (успешно удалено)
        }

    }
}
