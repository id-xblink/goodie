using GoodsAndOrders.DTOs;
using GoodsAndOrders.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoodsAndOrders.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly UserRoleService _userRoleService;

        public UserRoleController(UserRoleService userService)
        {
            _userRoleService = userService;
        }

        // Получить всех пользователей
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var roles = await _userRoleService.GetAllUserRolesAsync();
            return Ok(roles);
        }
    }
}
