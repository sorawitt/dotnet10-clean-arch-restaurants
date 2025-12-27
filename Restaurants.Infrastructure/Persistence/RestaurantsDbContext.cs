using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;

namespace Restaurants.Infrastructure.Persistence;

internal class RestaurantsDbContext(DbContextOptions<RestaurantsDbContext> options) : IdentityDbContext<User>(options)
{
    internal DbSet<Restaurant> Restaurants { get; set; } = null!;
    internal DbSet<Dish> Dishes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(r => r.Description)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(r => r.Category)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(r => r.ContactEmail)
                .HasMaxLength(100);

            entity.Property(r => r.ContactNumber)
                .HasMaxLength(20);

            entity.OwnsOne(r => r.Address, address =>
            {
                address.Property(a => a.City)
                    .HasMaxLength(100);

                address.Property(a => a.Street)
                    .HasMaxLength(200);

                address.Property(a => a.PostalCode)
                    .HasMaxLength(20);
            });

            entity.HasMany(r => r.Dishes)
                .WithOne()
                .HasForeignKey(d => d.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Dish>(entity =>
        {
            entity.HasKey(d => d.Id);

            entity.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(d => d.Description)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(d => d.Price)
                .IsRequired()
                .HasPrecision(10, 2);

            entity.Property(d => d.RestaurantId)
                .IsRequired();

            entity.HasIndex(d => d.RestaurantId);
        });
    }
}
