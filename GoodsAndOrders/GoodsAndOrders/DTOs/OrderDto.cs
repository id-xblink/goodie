namespace GoodsAndOrders.DTOs
{
    public class OrderDto
    {
        public Guid Id { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? ShipmentDate { get; set; }

        public int OrderNumber { get; set; }

        public Guid CustomerId { get; set; }

        public Guid StatusId { get; set; }

        // 🔹 Список товаров в заказе
        public List<OrderProductDto> Items { get; set; } = new();
    }

    public class OrderProductDto
    {
        public Guid ProductId { get; set; } // ID товара
        public string ProductName { get; set; } = string.Empty; // Название товара
        public decimal Price { get; set; } // Цена товара
        public int Quantity { get; set; } // Количество
    }
}
