using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>(e =>
            {
                e.Property(p => p.Price).HasColumnType("decimal(18,2)");
                e.HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Order>(e =>
            {
                e.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");
                e.Property(o => o.Status).HasConversion<string>();
                e.HasOne(o => o.User).WithMany(u => u.Orders).HasForeignKey(o => o.UserId);
            });


            builder.Entity<OrderItem>(e =>
            {
                e.Property(o => o.UnitPrice).HasColumnType("decimal(18,2)");
            });

            builder.Entity<Cart>(e =>
            {
                e.HasOne(c => c.User).WithOne(u => u.Cart).HasForeignKey<Cart>(c => c.UserId);
            });

            builder.Entity<CartItem>(e =>
            {
                e.HasOne(ci => ci.Cart).WithMany(c => c.Items).HasForeignKey(ci => ci.CartId);
                e.HasOne(ci => ci.Product).WithMany(p => p.CartItems).HasForeignKey(ci => ci.ProductId).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<OrderItem>(e =>
            {
                e.HasOne(oi => oi.Order).WithMany(o => o.Items).HasForeignKey(oi => oi.OrderId);
                e.HasOne(oi => oi.Product).WithMany(p => p.OrderItems).HasForeignKey(oi => oi.ProductId).OnDelete(DeleteBehavior.Restrict);
            });
        }

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    }
}
