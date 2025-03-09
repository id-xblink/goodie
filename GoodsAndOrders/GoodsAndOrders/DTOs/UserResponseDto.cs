namespace GoodsAndOrders.DTOs
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int Discount { get; set; }
        public string UserRole { get; set; } = string.Empty;
    }
}
