using GoodsAndOrders.Abstractions.Repositories;
using GoodsAndOrders.Data.Entities;
using System.Threading.Tasks;

namespace GoodsAndOrders.UnitOfWork
{
    public interface IUnitOfWork
    {
        IGenericRepository<OrderStatus> OrderStatuses { get; }
        IGenericRepository<UserRole> UserRoles { get; }
        Task<int> SaveChangesAsync();
    }
}