

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
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /** Need to override default behaviour and stop EF from using Output clauses if we want to work with Triggers. 
             *  See - https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/breaking-changes?tabs=v7#sqlserver-tables-with-triggers
             */
            modelBuilder.Entity<Product>()
                .ToTable(tb => tb.UseSqlOutputClause(false));

            modelBuilder.Entity<Purchase>()
                .ToTable(tb => tb.UseSqlOutputClause(false));

            modelBuilder.Entity<Customer>()
                .ToTable(tb => tb.UseSqlOutputClause(false));

            // Define DB relationships
            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.HasOne(p => p.Product)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(p => p.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Customer)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(p => p.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }   
}
