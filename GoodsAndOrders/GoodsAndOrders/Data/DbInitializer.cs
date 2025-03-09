using GoodsAndOrders.Data.Entities;

namespace GoodsAndOrders.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            // Проверяем, есть ли роли
            if (!context.UserRoles.Any())
            {
                context.UserRoles.AddRange(
                    new UserRole { Id = Guid.NewGuid(), Name = "Админ" },
                    new UserRole { Id = Guid.NewGuid(), Name = "Менеджер" },
                    new UserRole { Id = Guid.NewGuid(), Name = "Заказчик" }
                );
                context.SaveChanges();
            }

            // Проверяем, есть ли статусы заказов
            if (!context.OrderStatuses.Any())
            {
                context.OrderStatuses.AddRange(
                    new OrderStatus { Id = Guid.NewGuid(), Name = "Новый" },
                    new OrderStatus { Id = Guid.NewGuid(), Name = "Выполняется" },
                    new OrderStatus { Id = Guid.NewGuid(), Name = "Выполнен" }
                );
                context.SaveChanges();
            }
        }
    }
}