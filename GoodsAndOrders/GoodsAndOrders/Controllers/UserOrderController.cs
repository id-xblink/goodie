using GoodsAndOrders.DTOs;
using GoodsAndOrders.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoodsAndOrders.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class UserOrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public UserOrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        // Получить все заказы
        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] Guid? customerId)
        {
            var orders = await _orderService.GetAllOrdersAsync(customerId);
            return Ok(orders);
        }

        // Получить заказ по ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();

            return Ok(order);
        }

        // Создать новый заказ
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            if (orderDto == null) return BadRequest("Некорректные данные");

            var order = await _orderService.CreateOrderAsync(orderDto);
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] UpdateOrderDto updateOrderDto)
        {
            if (updateOrderDto == null) return BadRequest("Данные отсутствуют"); // Проверка входных данных

            bool updated = await _orderService.UpdateOrderAsync(id, updateOrderDto);
            if (!updated) return NotFound(); // Если товар не найден, возвращаем 404

            return NoContent(); // HTTP 204 (успешно, но без контента)
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] UpdateOrderDto updateDto)
        {
            if (updateDto == null) return BadRequest("Некорректные данные");

            bool success = await _orderService.UpdateOrderStatusAsync(id, updateDto.StatusId, updateDto.ShipmentDate);
            if (!success) return BadRequest("Ошибка обновления статуса заказа");

            return Ok("Статус заказа обновлён");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            bool deleted = await _orderService.DeleteOrderAsync(id);
            if (!deleted) return BadRequest("❌ Заказ нельзя удалить или его не существует.");
            return NoContent(); // Успешное удаление (204 No Content)

        }
    }
}
