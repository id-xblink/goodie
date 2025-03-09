using GoodsAndOrders.Data;
using GoodsAndOrders.Data.Entities;
using GoodsAndOrders.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GoodsAndOrders.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        // Получить все категории
        public async Task<List<ProductCategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _context.ProductCategories.ToListAsync();

            return categories.Select(c => new ProductCategoryDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
        }

        // Получить категорию по ID
        public async Task<ProductCategoryDto?> GetCategoryByIdAsync(Guid id)
        {
            var category = await _context.ProductCategories.FindAsync(id);
            if (category == null) return null;

            return new ProductCategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        // Добавить новую категорию
        public async Task<ProductCategoryDto> CreateCategoryAsync(ProductCategoryDto categoryDto)
        {
            var category = new ProductCategory
            {
                Id = Guid.NewGuid(),
                Name = categoryDto.Name
            };

            _context.ProductCategories.Add(category);
            await _context.SaveChangesAsync();

            return categoryDto;
        }

        public async Task<bool> UpdateCategoryAsync(Guid id, ProductCategoryDto categoryDto)
        {
            var category = await _context.ProductCategories.FindAsync(id);
            if (category == null) return false; // Если категории нет, возвращаем false

            // Обновляем данные
            category.Name = categoryDto.Name;

            await _context.SaveChangesAsync(); // Сохраняем изменения
            return true;
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            var category = await _context.ProductCategories.FindAsync(id);
            if (category == null) return false; // Если категории нет, возвращаем false

            _context.ProductCategories.Remove(category); // Удаляем категорию
            await _context.SaveChangesAsync(); // Сохраняем изменения
            return true;
        }

    }
}
