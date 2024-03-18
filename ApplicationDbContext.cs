using Microsoft.EntityFrameworkCore;
using PerformancePaging.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformancePaging;

public class ApplicationDbContext : DbContext
{
    public DbSet<Item> Items { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=127.0.0.1,5433;database=test_db;Trusted_Connection=True;Integrated Security=false;User Id=sa;password=Pass@word;TrustServerCertificate=true");
        optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>(item =>
        {
            item.ToTable("Item");
            item.Property(o => o.Id).HasColumnName("Id");
            item.Property(o => o.Name).HasColumnName("Name");
        });
    }
}
