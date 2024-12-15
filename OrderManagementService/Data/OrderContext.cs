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

        public DbSet<OrderLine> OrderLines { get; set; }

        // Configuring the entity using Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the Id property of Order and OrderLine to be auto-incremented
            modelBuilder.Entity<Order>()
                .Property(o => o.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<OrderLine>()
                .Property(ol => ol.Id)
                .ValueGeneratedOnAdd(); // Ensure OrderLine.Id is also auto-incremented

            // Configure one-to-many relationship between Order and OrderLine
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderLines)
                .WithOne(ol => ol.Order)
                .HasForeignKey(ol => ol.OrderId)
                .OnDelete(DeleteBehavior.Cascade);  // Optional: Specify cascade delete behavior if needed

            base.OnModelCreating(modelBuilder);
        }
    }
}
