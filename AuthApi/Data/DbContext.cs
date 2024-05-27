using Microsoft.EntityFrameworkCore;
using AuthApi.Models;
namespace AuthApi.Data
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options) : base(options) { }
        public DbSet<UserModel> Users { get; set; }
    }
}
