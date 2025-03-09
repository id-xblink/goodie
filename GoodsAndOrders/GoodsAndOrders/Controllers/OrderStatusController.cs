using GoodsAndOrders.Data.Entities;
using GoodsAndOrders.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

//using GoodsAndOrders.Data; // Пространство имён твоего проекта
//using GoodsAndOrders.Repositories; // Пространство имён с Unit of Work

namespace GoodsAndOrders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderStatusController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderStatusController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderStatus>>> GetAllStatuses()
        {
            var statuses = await _unitOfWork.OrderStatuses.GetAllAsync();
            return Ok(statuses);
        }
    }
}
