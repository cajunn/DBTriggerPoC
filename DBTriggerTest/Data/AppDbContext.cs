

using DBTriggerTest.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace DBTriggerTest.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            :base(options) { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .ToTable(tb => tb.UseSqlOutputClause(false));
        }
    }   
}
