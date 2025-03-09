namespace GoodsAndOrders.DTOs
{
    public class UpdateOrderDto
    {
        public Guid StatusId { get; set; } // Только статус
        public DateTime? ShipmentDate { get; set; } // Дата может быть пустой
    }
}
