namespace GoodsAndOrders.DTOs
{
    public class UpdateUserDto
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int Discount { get; set; }
        public Guid? CustomerId { get; set; }
    }
}
