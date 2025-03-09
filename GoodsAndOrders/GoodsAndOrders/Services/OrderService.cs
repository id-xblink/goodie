using GoodsAndOrders.Data;
using GoodsAndOrders.Data.Entities;
using GoodsAndOrders.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata.Ecma335; // 👈 Добавь этот using

namespace GoodsAndOrders.Services
{
    public class OrderService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration; // 👈 Добавляем _configuration

        public OrderService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration; // 👈 Присваиваем
        }

        // Получить все заказы
        public async Task<List<OrderResponseDto>> GetAllOrdersAsync(Guid? customerId)
        {
            var query = _context.UserOrders
                .Include(o => o.OrderStatus)
                .AsQueryable();

            if (customerId.HasValue)
            {
                query = query.Where(o => o.CustomerId == customerId.Value);
            }

            var orders = await query
                .Select(o => new OrderResponseDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    OrderNumber = o.OrderNumber,
                    ShipmentDate = o.ShipmentDate,
                    Status = o.OrderStatus.Name
                })
                .ToListAsync();

            return orders;

            //var orders = await _context.UserOrders.ToListAsync();
            //return orders.Select(o => new OrderDto
            //{
            //    Id = o.Id,
            //    OrderDate = o.OrderDate,
            //    ShipmentDate = o.ShipmentDate,
            //    OrderNumber = o.OrderNumber,
            //    CustomerId = o.CustomerId,
            //    StatusId = o.StatusId
            //}).ToList();
        }

        public async Task<OrderDto?> GetOrderByIdAsync(Guid id)
        {
            var order = await _context.UserOrders
                .Include(o => o.OrderElements)  // Загружаем товары в заказе
                .ThenInclude(puo => puo.Product) // Загружаем сами товары
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return null;

            return new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                ShipmentDate = order.ShipmentDate,
                OrderNumber = order.OrderNumber,
                CustomerId = order.CustomerId,
                StatusId = order.StatusId,
                Items = order.OrderElements.Select(puo => new OrderProductDto
                {
                    ProductId = puo.ProductId,
                    ProductName = puo.Product.Name, // 🔹 Берём имя товара
                    Price = puo.ProductPrice, // 🔹 Цена на момент покупки
                    Quantity = puo.ProductCount // 🔹 Количество в заказе
                }).ToList()
            };
        }


        //// Получить заказ по ID
        //public async Task<OrderDto?> GetOrderByIdAsync(Guid id)
        //{
        //    var order = await _context.UserOrders.FindAsync(id);
        //    if (order == null) return null;

        //    return new OrderDto
        //    {
        //        Id = order.Id,
        //        OrderDate = order.OrderDate,
        //        ShipmentDate = order.ShipmentDate,
        //        OrderNumber = order.OrderNumber,
        //        CustomerId = order.CustomerId,
        //        StatusId = order.StatusId
        //    };
        //}

        // Создать заказ
        public async Task<OrderResponseDto> CreateOrderAsync(CreateOrderDto orderDto)
        {
            var newStatusName = _configuration["DefaultStatuses:New"];

            var orderStatus = await _context.OrderStatuses
                .FirstOrDefaultAsync(s => s.Name == newStatusName); // Загружаем весь объект

            // Загружаем заказчика с его скидкой
            var customer = await _context.Users
                .Where(u => u.Id == orderDto.CustomerId)
                .Select(u => new { u.Id, u.Discount })
                .FirstOrDefaultAsync();

            var order = new UserOrder
            {
                Id = Guid.NewGuid(),
                OrderDate = DateTime.UtcNow,
                OrderNumber = await _context.UserOrders.CountAsync() + 1, // Авто-номер заказа
                CustomerId = orderDto.CustomerId,
                StatusId = orderStatus.Id,
                OrderStatus = orderStatus
            };

            _context.UserOrders.Add(order);
            await _context.SaveChangesAsync();

            // ✅ Создаём связи "Товар—Заказ"
            var orderProducts = orderDto.Items.Select(item =>
            {
                var productPrice = _context.Products
                                        .Where(p => p.Id == item.ProductId)
                                        .Select(p => p.Price)
                                        .FirstOrDefault(); // ✅ Записываем актуальную цену

                return new ProductUserOrder
                {
                    Id = Guid.NewGuid(),
                    UserOrderId = order.Id,
                    ProductId = item.ProductId,
                    ProductCount = item.Quantity,
                    //Product_Price = _context.Products
                    //                        .Where(p => p.Id == item.ProductId)
                    //                        .Select(p => p.Price)
                    //                        .FirstOrDefault() // ✅ Записываем актуальную цену
                    ProductPrice = CalculateDiscountedPrice(productPrice, customer.Discount),
                };
            }).ToList();
            
            //new ProductUserOrder
            //{
            //    var _context.Products
            //                            .Where(p => p.Id == item.ProductId)
            //                            .Select(p => p.Price)
            //                            .FirstOrDefault() // ✅ Записываем актуальную цену

            //    Id = Guid.NewGuid(),
            //    UserOrderId = order.Id,
            //    ProductId = item.ProductId,
            //    ProductCount = item.Quantity,
            //    //Product_Price = _context.Products
            //    //                        .Where(p => p.Id == item.ProductId)
            //    //                        .Select(p => p.Price)
            //    //                        .FirstOrDefault() // ✅ Записываем актуальную цену
            //    ProductPrice = {


            //        return 1;
            //    },

            //}).ToList();

            _context.ProductsUserOrders.AddRange(orderProducts);
            await _context.SaveChangesAsync(); // ✅ Сохраняем связи

            return new OrderResponseDto()
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                OrderNumber = order.OrderNumber,
                Status = order.OrderStatus.Name, // Отправляем название статуса
            };
        }

        private decimal CalculateDiscountedPrice(decimal originalPrice, decimal discount)
        {
            return originalPrice * (1 - (discount / 100m));
        }

        public async Task<bool> UpdateOrderAsync(Guid id, UpdateOrderDto updateDto)
        {
            var order = await _context.UserOrders.FindAsync(id);
            if (order == null) return false; // Если заказа нет, возвращаем false

            // Обновляем статус (теперь по GUID)
            order.StatusId = updateDto.StatusId;

            if (updateDto.ShipmentDate.HasValue)
                order.ShipmentDate = updateDto.ShipmentDate.Value;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateOrderStatusAsync(Guid orderId, Guid newStatusId, DateTime? shipmentDate)
        {
            var order = await _context.UserOrders.Include(o => o.OrderStatus).FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null) return false; // Если заказа нет — ничего не делаем

            var newStatus = await _context.OrderStatuses.FirstOrDefaultAsync(s => s.Id == newStatusId);
            if (newStatus == null) return false; // Если статус не найден — ошибка

            // 🟢 Новый → Выполняется
            if (order.OrderStatus.Name == "Новый" && newStatus.Name == "Выполняется")
            {
                if (!shipmentDate.HasValue || shipmentDate.Value.Date < DateTime.UtcNow.Date)
                    return false; // Дата отгрузки должна быть сегодня или позже

                order.ShipmentDate = shipmentDate.Value; // Записываем дату отгрузки
            }

            // 🔴 Выполняется → Выполнен
            if (order.OrderStatus.Name == "Выполняется" && newStatus.Name == "Выполнен")
            {
                if (!order.ShipmentDate.HasValue || order.ShipmentDate.Value.Date > DateTime.UtcNow.Date)
                    return false; // Нельзя закрыть заказ до даты отгрузки
            }

            order.StatusId = newStatusId;
            await _context.SaveChangesAsync(); // Сохраняем изменения
            return true;
        }


        public async Task<bool> DeleteOrderAsync(Guid id)
        {
            var order = await _context.UserOrders.Include(o => o.OrderStatus).FirstOrDefaultAsync(o => o.Id == id);

            if (order == null || order.OrderStatus.Name != "Новый")
                return false; // Заказ не найден или его нельзя удалить

            //if (order == null) return false; // Если заказа нет, возвращаем false

            _context.UserOrders.Remove(order); // Удаляем заказ
            await _context.SaveChangesAsync(); // Сохраняем изменения
            return true;
        }
    }
}
