using AuthApi.Data;
using AuthApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthContext _context;

        public AuthController(AuthContext context)
        {
            _context = context;
        }

        [HttpGet("exists/{username}")]
        public async Task<IActionResult> UserExists(string username)
        {
            var user = await _context.Users.AnyAsync(u => u.Username == username);
            if (user)
            {
                return Ok(new { exists = true });
            }
            return Ok(new { exists = false });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (await _context.Users.AnyAsync(u => u.Username == model.Username || u.Email == model.Email))
            {
                return BadRequest("User already exists.");
            }

            var user = new UserModel
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { user.Id, user.Username, user.Email });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid username or password.");
            }

            // Generate JWT token (example implementation)
            var token = GenerateJwtToken(user);

            return Ok(new { token });
        }

        private string GenerateJwtToken(UserModel user)
        {
            // Реализация генерации JWT токена
            return "jwt_token_example";
        }
    }
}
