using GoodsAndOrders.Data.Entities;
using GoodsAndOrders.Data;
using Microsoft.EntityFrameworkCore;

namespace GoodsAndOrders.Repositories
{
    public class OrderStatusRepository : GenericRepository<OrderStatus>, IOrderStatusRepository
    {
        public OrderStatusRepository(AppDbContext context) : base(context) { }

        public async Task<OrderStatus?> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(os => os.Name == name);
        }
    }
}
