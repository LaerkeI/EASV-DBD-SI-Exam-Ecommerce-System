using MongoDB.Bson;
using Newtonsoft.Json;

namespace CatalogManagementService.Domain.Serialization
{
    public class ObjectIdJsonConverter : JsonConverter<ObjectId>
    {
        public override void WriteJson(JsonWriter writer, ObjectId value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());  // Convert ObjectId to string during serialization
        }

        public override ObjectId ReadJson(JsonReader reader, Type objectType, ObjectId existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = reader.Value as string;
            return value != null ? ObjectId.Parse(value) : ObjectId.Empty;  // Convert string back to ObjectId during deserialization
        }
    }
}