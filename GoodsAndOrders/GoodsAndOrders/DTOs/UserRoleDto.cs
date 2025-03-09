using GoodsAndOrders.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace GoodsAndOrders.DTOs
{
    public class UserRoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
