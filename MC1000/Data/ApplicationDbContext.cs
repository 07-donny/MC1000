using System;
using System.Collections.Generic;
using System.Text;
using MC1000.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MC1000.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
            .HasMany(p => p.SubCategories)
            .WithOne(c => c.Category)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SubCategory>()
            .HasMany(p => p.SubSubCategories)
            .WithOne(c => c.SubCategory)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
            .HasOne(p => p.SubSubCategory)
            .WithMany(o => o.Products)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Promotion>()
            .HasMany(p => p.Discounts)
            .WithOne(c => c.Promotion)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DeliverySlot>()
            .HasMany(p => p.TimeSlots)
            .WithOne(c => c.DeliverySlot)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
            .HasMany(p => p.OrderLines)
            .WithOne(c => c.Order)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderLine>()
            .HasOne(p => p.Product)
            .WithMany(o => o.OrderLines)
            .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<DeliverySlot> DeliverySlot { get; set; }
        public DbSet<Discount> Discount { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderLine> OrderLine { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Promotion> Promotion { get; set; }
        public DbSet<TimeSlot> TimeSlot { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<SubCategory> SubCategory { get; set; }
        public DbSet<SubSubCategory> SubSubCategory { get; set; }
        public DbSet<HomeBanner> HomeBanner { get; set; }
    }
}