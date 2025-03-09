using GoodsAndOrders.Data;
using GoodsAndOrders.Data.Entities;
using GoodsAndOrders.DTOs;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using GoodsAndOrders.Utils;

namespace GoodsAndOrders.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        // Получить всех пользователей
        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Code = u.Code,
                Address = u.Address,
                Discount = u.Discount,
                UserRoleId = u.UserRoleId
            }).ToList();
        }

        public async Task<List<UserDto>> GetAllManagersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Code = u.Code,
                Address = u.Address,
                Discount = u.Discount,
                UserRoleId = u.UserRoleId
            }).ToList();
        }

        // Получить всех пользователей для редактирования
        public async Task<object> GetAllUsersForEditAsync(int page, int pageSize, string? search, Guid? userRoleId)
        {
            var query = _context.Users
                .Include(u => u.UserRole)
                .AsQueryable();

            //var query = _context.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Code.Contains(search));

            if (userRoleId.HasValue)
                query = query.Where(p => p.UserRoleId == userRoleId.Value);

            int totalItems = await query.CountAsync();

            var users = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserResponseDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Code = u.Code,
                    Address = u.Address,
                    Discount = u.Discount,
                    UserRole = u.UserRole.Name
                })
                .ToListAsync();

            //return users;

            return new
            {
                users,
                totalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                totalItems
            };
        }
        //public async Task<List<UserResponseDto>> GetAllUsersForEditAsync(int page, int pageSize, string? search, Guid? userRoleId)
        //{
        //    var query = _context.Users
        //        .Include(u => u.UserRole)
        //        .AsQueryable();

        //    //var query = _context.Products.Include(p => p.Category).AsQueryable();

        //    if (!string.IsNullOrWhiteSpace(search))
        //        query = query.Where(p => p.Code.Contains(search));

        //    if (userRoleId.HasValue)
        //        query = query.Where(p => p.UserRoleId == userRoleId.Value);

        //    int totalItems = await query.CountAsync();

        //    var users = await query
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .Select(u => new UserResponseDto
        //        {
        //            Id = u.Id,
        //            Name = u.Name,
        //            Code = u.Code,
        //            Address = u.Address,
        //            Discount = u.Discount,
        //            UserRole = u.UserRole.Name
        //        })
        //        .ToListAsync();

        //    return users;
        //}

        // Получить пользователя по ID
        public async Task<UserDto?> GetUserByIdAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Code = user.Code,
                Address = user.Address,
                Discount = user.Discount,
                UserRoleId = user.UserRoleId
            };
        }

        // Создать пользователя
        public async Task<UserDto> CreateUserAsync(UserDto userDto, string password)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = userDto.Name,
                Login = userDto.Login,
                Code = GenerateUserCode(),
                Address = userDto.Address,
                Discount = userDto.Discount,
                UserRoleId = userDto.UserRoleId,
                PasswordHash = PasswordHasher.HashPassword(password) // Хешируем пароль
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return userDto;
        }

        // Создать пользователя при регистрации
        public async Task<UserDto> CreateUserOnRegistrationAsync(UserDto userDto, string password)
        {
            var userRole = await _context.UserRoles.FirstOrDefaultAsync(r => r.Name == "Заказчик");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Login = userDto.Login,
                Name = userDto.Name,
                Code = GenerateUserCode(),
                Address = userDto.Address,
                Discount = 0,
                UserRoleId = userRole.Id,
                PasswordHash = PasswordHasher.HashPassword(password) // Хешируем пароль
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return userDto;
        }

        public async Task<bool> UpdateUserAsync(Guid id, UpdateUserDto updateDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false; // Если пользователь не найден, возвращаем false

            // Обновляем только разрешённые поля
            user.Name = updateDto.Name;
            user.Address = updateDto.Address;
            user.Discount = updateDto.Discount;

            await _context.SaveChangesAsync(); // Сохраняем изменения
            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false; // Если пользователя нет, возвращаем false

            _context.Users.Remove(user); // Удаляем пользователя
            await _context.SaveChangesAsync(); // Сохраняем изменения
            return true;
        }

        public string GenerateUserCode()
        {
            string year = DateTime.UtcNow.Year.ToString();

            // Найти максимальный Code с текущим годом
            var maxCode = _context.Users
                .Where(u => u.Code.EndsWith(year)) // Фильтруем по году
                .OrderByDescending(u => u.Code)    // Сортируем по убыванию
                .Select(u => u.Code)               // Берем Code
                .FirstOrDefault();

            int nextNumber = 1; // Если записей нет, начинаем с 1

            if (maxCode != null)
            {
                // Извлекаем число перед годом
                string[] parts = maxCode.Split('-');
                if (parts.Length == 2 && int.TryParse(parts[0], out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            return $"{nextNumber:D4}-{year}"; // Форматируем с 4 нулями (например, 0001-2024)
            //return $"{year}-{2228}";
        }
    }
}
