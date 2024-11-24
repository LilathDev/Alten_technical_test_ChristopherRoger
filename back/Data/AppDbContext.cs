using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace back.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        /// <summary>
        /// Configures the model for the database using the ModelBuilder.
        /// </summary>
        /// <param name="modelBuilder">The model builder used to configure entity models.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                // Set primary key
                entity.HasKey(u => u.Id);

                // Configure properties
                entity.Property(u => u.Username)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(u => u.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(256);

                // One-to-one relationship with Cart
                entity.HasOne(u => u.Cart)
                    .WithOne(c => c.User)
                    .HasForeignKey<Cart>(c => c.UserId)
                    .IsRequired();
            });

            // Configure Cart entity
            modelBuilder.Entity<Cart>(entity =>
            {
                // Set primary key
                entity.HasKey(c => c.Id);

                // Configure one-to-one relationship with User
                entity.HasOne(c => c.User)
                    .WithOne(u => u.Cart)
                    .HasForeignKey<Cart>(c => c.UserId)
                    .IsRequired();

                // Configure default values for timestamps
                entity.Property(c => c.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(c => c.UpdatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

        }

    }
}