using GoodsAndOrders.Data;
using GoodsAndOrders.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace GoodsAndOrders.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            await _context.Users.Include(u => u.UserRole).ForEachAsync(u =>
            {
                Console.WriteLine(u.UserRoleId);
            });

            var user = await _context.Users
                .Include(u => u.UserRole)
                .FirstOrDefaultAsync(u => u.Login == request.Login);

            //var user = await _context.Users
            //    .Include(u => u.UserRole)
            //    .Where(u => u.Login == request.Login)
            //    .FirstOrDefaultAsync();

            //Console.WriteLine(user.UserRoleId);
            //Console.WriteLine(user.UserRole.Name);
            //Console.WriteLine(request.Login);
            //Console.WriteLine(request.Password);


            if (user == null)
                return Unauthorized(new { message = "Пользователь не найден" });

            if (!PasswordHasher.VerifyPassword(request.Password, user.PasswordHash))
                return Unauthorized(new { message = "Неверный пароль" });

            //Console.WriteLine(user.UserRole.Name.ToString());

            var token = _jwtService.GenerateToken(user.Id, user.UserRole?.Name ?? "User", user.Name);

            return Ok(new { token });
        }
    }

    public class LoginRequest
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
