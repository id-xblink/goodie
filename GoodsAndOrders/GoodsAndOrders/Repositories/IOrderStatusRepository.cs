using GoodsAndOrders.Abstractions.Repositories;
using GoodsAndOrders.Data.Entities;

namespace GoodsAndOrders.Repositories
{
    public interface IOrderStatusRepository : IGenericRepository<OrderStatus>
    {
        Task<OrderStatus?> GetByNameAsync(string name);
    }
}
