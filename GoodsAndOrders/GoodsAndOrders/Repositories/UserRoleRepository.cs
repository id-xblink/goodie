using GoodsAndOrders.Data.Entities;
using GoodsAndOrders.Data;
using Microsoft.EntityFrameworkCore;

namespace GoodsAndOrders.Repositories
{
    public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(AppDbContext context) : base(context) { }

        public async Task<UserRole?> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(ur => ur.Name == name);
        }
    }
}
