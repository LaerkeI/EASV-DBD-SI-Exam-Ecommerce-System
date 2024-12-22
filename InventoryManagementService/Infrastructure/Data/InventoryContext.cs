using InventoryManagementService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementService.Infrastructure.Data
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options)
        {
        }

        public DbSet<InventoryItem> InventoryItems { get; set; }
    }
}
