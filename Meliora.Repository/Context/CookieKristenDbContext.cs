using Meliora.Domain.Models.CookiesKristen;
using Microsoft.EntityFrameworkCore;

namespace Meliora.Repository.Context;

public class CookieKristenDbContext(DbContextOptions<CookieKristenDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Mixin> Mixins { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // only for Migrations
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=db,1433;Database=CookieKristen;User=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<Order>()
            .HasKey(o => o.Id);

        modelBuilder.Entity<Mixin>()
            .HasKey(m => m.Id);
    }
}
