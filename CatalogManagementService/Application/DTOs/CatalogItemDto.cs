namespace CatalogManagementService.Application.DTOs
{
    public class CatalogItemDto
    {
        public string ItemId { get; set; } // Contains the same value as ItemId in OrderManagementService and InventoryManagementService. Could be ISBN, SKU or barcode.
        public string Name { get; set; }
        public string Description { get; set; }
        public string Producer { get; set; } // Someone who creates a good or service. For books = the author
        public string Manufacturer { get; set; } // Someone who takes raw materials and transforms them into a finished product. For books = the publisher
        public bool IsAvailable { get; set; }
    }
}
