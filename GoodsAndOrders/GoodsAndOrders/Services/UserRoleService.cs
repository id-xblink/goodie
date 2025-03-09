using GoodsAndOrders.Data.Entities;
using GoodsAndOrders.Data;
using GoodsAndOrders.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GoodsAndOrders.Services
{
    public class UserRoleService
    {
        private readonly AppDbContext _context;

        public UserRoleService(AppDbContext context)
        {
            _context = context;
        }

        // Получить всех пользователей
        public async Task<List<UserRoleDto>> GetAllUserRolesAsync()
        {
            var roles = await _context.UserRoles.ToListAsync();
            return roles.Select(u => new UserRoleDto
            {
                Id = u.Id,
                Name = u.Name,
            }).ToList();
        }
    }
}