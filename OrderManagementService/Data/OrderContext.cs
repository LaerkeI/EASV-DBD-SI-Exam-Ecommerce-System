using Microsoft.EntityFrameworkCore;
using OrderManagementService.Entities;

namespace OrderManagementService.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }

        // Configuring the entity using Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the Id property to be auto-incremented by the database
            modelBuilder.Entity<Order>()
                .Property(o => o.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
