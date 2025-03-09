using GoodsAndOrders.Data.Entities;
using GoodsAndOrders.DTOs;
using GoodsAndOrders.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoodsAndOrders.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        // Получить всех пользователей
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // Получить все заказы
        [HttpGet("managers")]
        public async Task<IActionResult> GetManagers()
        {
            var managers = await _userService.GetAllManagersAsync();
            return Ok(managers);
        }

        // Получить всех пользователей
        [HttpGet("dto")]
        public async Task<IActionResult> GetAllUsersForEdit(int page = 1, int pageSize = 10, string? search = null, Guid? userRoleId = null)
        {
            if (page < 1 || pageSize < 1) return BadRequest("Некорректные параметры пагинации");

            var users = await _userService.GetAllUsersForEditAsync(page, pageSize, search, userRoleId);
            return Ok(users);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetProducts(int page = 1, int pageSize = 10, string? search = null, decimal? minPrice = null, decimal? maxPrice = null, Guid? categoryId = null)
        //{
        //    //Console.WriteLine("hit controller");
        //    if (page < 1 || pageSize < 1) return BadRequest("Некорректные параметры пагинации");

        //    var result = await _productService.GetProductsAsync(page, pageSize, search, minPrice, maxPrice, categoryId);
        //    return Ok(result);
        //}

        // Получить пользователя по ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            return Ok(user);
        }

        // Создать нового пользователя
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto, [FromQuery] string password)
        {
            if (userDto == null || string.IsNullOrWhiteSpace(password))
                return BadRequest("Некорректные данные");

            var user = await _userService.CreateUserAsync(userDto, password);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPost("registration")]
        public async Task<IActionResult> CreateUserOnRegister([FromBody] UserDto userDto, [FromQuery] string password)
        {
            if (userDto == null || string.IsNullOrWhiteSpace(password))
                return BadRequest("Некорректные данные");

            var user = await _userService.CreateUserOnRegistrationAsync(userDto, password);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto updateUserDto)
        {
            if (updateUserDto == null)
                return BadRequest("Данные отсутствуют");

            bool updated = await _userService.UpdateUserAsync(id, updateUserDto);
            if (!updated)
                return NotFound(); // Если пользователь не найден, возвращаем 404

            return NoContent(); // HTTP 204 (успешно, но без контента)
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            bool deleted = await _userService.DeleteUserAsync(id);
            if (!deleted) return NotFound(); // Если пользователя нет, возвращаем 404

            return NoContent(); // HTTP 204 (успешно удалено)
        }
    }
}
