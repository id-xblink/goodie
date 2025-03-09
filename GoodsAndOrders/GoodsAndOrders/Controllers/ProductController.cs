using GoodsAndOrders.Data.Entities;
using GoodsAndOrders.DTOs;
using GoodsAndOrders.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoodsAndOrders.Controllers
{
    [Authorize] // ❗ Все методы требуют авторизацию
    [Route("api/products")]  // Базовый маршрут для API
    [ApiController]  // Указывает, что это API-контроллер
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        // Получить все товары
        [HttpGet("all")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products); // Возвращаем HTTP 200 с JSON
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(int page = 1, int pageSize = 10, string? search = null, decimal? minPrice = null, decimal? maxPrice = null, Guid? categoryId = null)
        {
            //Console.WriteLine("hit controller");
            if (page < 1 || pageSize < 1) return BadRequest("Некорректные параметры пагинации");

            var result = await _productService.GetProductsAsync(page, pageSize, search, minPrice, maxPrice, categoryId);
            return Ok(result);
        }


        // Получить товар по ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound(); // HTTP 404, если товар не найден
            return Ok(product);
        }

        // Добавить новый товар
        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> CreateProduct([FromBody] UpdateProductDto productDto)
        {
            if (string.IsNullOrWhiteSpace(productDto.Name) || productDto.Price <= 0 || productDto.CategoryId == Guid.Empty)
                return BadRequest(new { message = "Некорректные данные товара" });

            //var category = await _context.ProductCategories.FindAsync(product.CategoryId);
            //if (category == null)
            //    return BadRequest(new { message = "Категория не найдена" });

            if (productDto == null) return BadRequest("Данные отсутствуют"); // HTTP 400
            var createdProduct = await _productService.CreateProductAsync(productDto);
            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
        }
        [Authorize(Roles = "Manager,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductDto productDto)
        {
            if (productDto == null) return BadRequest("Данные отсутствуют"); // Проверка входных данных

            bool updated = await _productService.UpdateProductAsync(id, productDto);
            if (!updated) return NotFound(); // Если товар не найден, возвращаем 404

            return NoContent(); // HTTP 204 (успешно, но без контента)
        }
        [Authorize(Roles = "Manager,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            bool deleted = await _productService.DeleteProductAsync(id);
            if (!deleted) return NotFound(); // Если товар не найден, возвращаем 404

            return NoContent(); // HTTP 204 (успешно удалено)
        }
    }
}
