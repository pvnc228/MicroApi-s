using AuthApi.Data;
using AuthApi.Interface;
using AuthApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI
{
    public class AuthService : IAuthService
    {
        private readonly AuthContext _context;

        public AuthService(AuthContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, string Message)> RegisterAsync(RegisterModel model)
        {
            if (await _context.Users.AnyAsync(u => u.Username == model.Username || u.Email == model.Email))
            {
                return (false, "User already exists.");
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

            return (true, "User registered successfully.");
        }

        public async Task<(bool Success, string Token)> LoginAsync(LoginModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                return (false, null);
            }

            // Generate JWT token (example implementation)
            var token = GenerateJwtToken(user);

            return (true, token);
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        private string GenerateJwtToken(UserModel user)
        {
            // Реализация генерации JWT токена
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("yoursecretkey");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = "yourissuer",
                Audience = "youraudience"
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
