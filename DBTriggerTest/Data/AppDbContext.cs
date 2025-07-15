

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


        /** Need to override default behaviour and stop EF from using Output clauses if we want to work with Triggers. 
         *  See - https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/breaking-changes?tabs=v7#sqlserver-tables-with-triggers
         */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .ToTable(tb => tb.UseSqlOutputClause(false));
        }
    }   
}
