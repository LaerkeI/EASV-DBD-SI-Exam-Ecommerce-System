using CatalogManagementService.Domain.Serialization;
using Newtonsoft.Json;

namespace CatalogManagementService.Infrastructure.Utils
{
    public class JsonSettingsProvider
    {
        public static JsonSerializerSettings GetJsonSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new ObjectIdJsonConverter() }
            };
        }
    }
}

