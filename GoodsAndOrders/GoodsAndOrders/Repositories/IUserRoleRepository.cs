using GoodsAndOrders.Abstractions.Repositories;
using GoodsAndOrders.Data.Entities;

namespace GoodsAndOrders.Repositories
{
    public interface IUserRoleRepository : IGenericRepository<UserRole>
    {
        Task<UserRole?> GetByNameAsync(string name);
    }
}
