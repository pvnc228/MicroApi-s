using Microsoft.EntityFrameworkCore;
using NotesService.Models;

namespace NotesService.Context
{
    public class NotesContext : DbContext
    {
        public NotesContext(DbContextOptions<NotesContext> options) : base(options) { }

        public DbSet<StoredNote> StoredNotes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StoredNote>().ToTable("StoredNotes");
        }
    }
}
