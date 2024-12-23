using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CatalogManagementService.Domain.Entities
{
    public class CatalogItem
    {
        [BsonId] // Marks this property as the MongoDB _id field
        public ObjectId BsonId { get; set; }

        [BsonElement("ItemId")] 
        public string ItemId { get; set; } // ISBN/SKU/Barcode

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Description")]
        public string Description { get; set; }

        [BsonElement("Producer")]
        public string Producer { get; set; } // Someone who creates a good or service. For books = the author

        [BsonElement("Manufacturer")]
        public string Manufacturer { get; set; } // Someone who takes raw materials and transforms them into a finished product. For books = the publisher

        [BsonElement("IsAvailable")]
        public bool IsAvailable { get; set; }
    }
}
