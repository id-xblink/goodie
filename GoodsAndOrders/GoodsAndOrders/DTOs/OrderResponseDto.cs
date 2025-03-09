namespace GoodsAndOrders.DTOs
{
    public class OrderResponseDto
    {
        public Guid Id { get; set; } // ID заказа
        public DateTime OrderDate { get; set; } // Дата создания
        public DateTime? ShipmentDate { get; set; } // Дата создания
        public int OrderNumber { get; set; } // Номер заказа
        public string Status { get; set; } = string.Empty; // Название статуса
    }
}
