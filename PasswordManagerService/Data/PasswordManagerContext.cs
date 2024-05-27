using Microsoft.EntityFrameworkCore;
using PasswordManagerService.Models;

namespace PasswordManagerService.Data
{
    public class PasswordManagerContext : DbContext
    {
        public PasswordManagerContext(DbContextOptions<PasswordManagerContext> options) : base(options) { }

        public DbSet<StoredPassword> StoredPasswords { get; set; }
        public DbSet<StoredMedia> StoredMedia { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StoredPassword>().ToTable("StoredPasswords");
            modelBuilder.Entity<StoredMedia>().ToTable("StoredMedia");
        }
    }
}
