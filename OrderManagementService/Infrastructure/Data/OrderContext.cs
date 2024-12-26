using Microsoft.EntityFrameworkCore;
using OrderManagementService.Domain.Entities;

namespace OrderManagementService.Infrastructure.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }


        // Configuring the entity using Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Explicitly configure the primary key for Order as Id
            modelBuilder.Entity<Order>()
                .HasKey(o => o.OrderId);

            // Configure the Id property of Order to be auto-incremented
            modelBuilder.Entity<Order>()
                .Property(o => o.OrderId)
                .ValueGeneratedOnAdd();

            // Configure composite key for OrderLine
            modelBuilder.Entity<OrderLine>()
                .HasKey(ol => new { ol.OrderId, ol.ItemId }); // Composite Key (OrderId, ItemId)

            // Configure one-to-many relationship between Order and OrderLine
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderLines)
                .WithOne(ol => ol.Order)
                .HasForeignKey(ol => ol.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
