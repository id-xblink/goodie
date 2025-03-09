namespace GoodsAndOrders.DTOs
{
    public class CreateOrderDto
    {
        public Guid CustomerId { get; set; }
        public List<ProductOrderDto> Items { get; set; } = new(); // Товары в заказе
    }

    public class ProductOrderDto
    {
        public Guid ProductId { get; set; } // ID товара
        public int Quantity { get; set; } // Количество
    }
}
