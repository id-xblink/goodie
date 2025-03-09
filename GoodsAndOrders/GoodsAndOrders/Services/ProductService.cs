using GoodsAndOrders.Data;
using GoodsAndOrders.Data.Entities;
using GoodsAndOrders.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GoodsAndOrders.Services
{
    public class ProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        // Получить все товары
        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            var products = await _context.Products.Include(p => p.Category).ToListAsync();

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CategoryName = p.Category.Name
            }).ToList();
        }

        public async Task<object> GetProductsAsync(int page, int pageSize, string? search, decimal? minPrice, decimal? maxPrice, Guid? categoryId)
        {
            //Console.WriteLine("hit service");
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Name.Contains(search));

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            int totalItems = await query.CountAsync();
            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new
            {
                items = products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    CategoryName = p.Category.Name
                }),
                totalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                totalItems
            };
        }


        // Получить товар по ID
        public async Task<ProductDto?> GetProductByIdAsync(Guid id)
        {
            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                CategoryName = product.Category.Name      
            };
        }

        // Добавить новый товар
        public async Task<UpdateProductDto> CreateProductAsync(UpdateProductDto productDto)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = productDto.Name,
                Price = productDto.Price,
                Code = GenerateProductCode(),
                CategoryId = productDto.CategoryId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return productDto;
        }

        //Обновить товар
        public async Task<bool> UpdateProductAsync(Guid id, UpdateProductDto productDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false; // Если товар не найден

            // Обновляем данные
            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.CategoryId = productDto.CategoryId;

            await _context.SaveChangesAsync(); // Сохраняем изменения
            return true;
        }

        // Удалить товар
        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false; // Если товара нет

            _context.Products.Remove(product); // Удаляем товар
            await _context.SaveChangesAsync(); // Сохраняем изменения
            return true;
        }

        private static readonly Random _random = new Random();

        public string GenerateProductCode()
        {
            int part1 = _random.Next(0, 100);     // XX (0–99)
            int part2 = _random.Next(0, 10000);   // XXXX (0–9999)
            string part3 = RandomLetters(2);      // YY (две буквы)
            int part4 = _random.Next(0, 100);     // XX (0–99)

            string newCode = $"{part1:D2}-{part2:D4}-{part3}{part4:D2}";

            // Проверяем уникальность
            while (_context.Products.Any(p => p.Code == newCode))
            {
                part1 = _random.Next(0, 100);
                part2 = _random.Next(0, 10000);
                part3 = RandomLetters(2);
                part4 = _random.Next(0, 100);
                newCode = $"{part1:D2}-{part2:D4}-{part3}{part4:D2}";
            }

            return newCode;
        }

        // Генерация случайных заглавных букв
        private static string RandomLetters(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
