using System;
using System.Collections.Generic;
using System.Text;
using MC1000.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MC1000.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Category { get; set; }
            public DbSet<DeliverySlot> DeliverySlot { get; set; }
            public DbSet<Discount> Discount { get; set; }
            public DbSet<News> News { get; set; }
            public DbSet<Order> Order { get; set; }
            public DbSet<OrderLine> OrderLine { get; set; }
            public DbSet<Product> Product { get; set; }
            public DbSet<Promotion> Promotion { get; set; }
            public DbSet<ShoppingCart> ShoppingCart { get; set; }
            public DbSet<SubCategory> SubCategory { get; set; }
            public DbSet<SubSubCategory> SubSubCategory { get; set; }
            public DbSet<TimeSlot> TimeSlot { get; set; }

            //public DbSet<User> User { get; set; }
    }
}
