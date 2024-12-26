using InventoryManagementService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementService.Infrastructure.Data
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options)
            : base(options)
        {
        }

        public DbSet<InventoryItem> InventoryItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<InventoryItem>(entity =>
            {
                entity.HasKey(e => e.ItemId);
                entity.Property(e => e.Quantity).IsRequired();
            });
        }
    }
}
