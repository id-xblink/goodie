using GoodsAndOrders.Abstractions.Repositories;
using GoodsAndOrders.Data;
using GoodsAndOrders.Data.Entities;

namespace GoodsAndOrders.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context;

        //public IOrderStatusRepository OrderStatuses { get; }
        //public IUserRoleRepository UserRoles { get; }

        public IGenericRepository<OrderStatus> OrderStatuses { get; }
        public IGenericRepository<UserRole> UserRoles { get; }

        public UnitOfWork(AppDbContext context, IGenericRepository<OrderStatus> orderStatusRepository, IGenericRepository<UserRole> userRoleRepository)
        {
            _context = context;
            OrderStatuses = orderStatusRepository;
            UserRoles = userRoleRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}