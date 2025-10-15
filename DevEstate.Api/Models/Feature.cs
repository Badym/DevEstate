using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace DevEstate.Api.Models
{
    public class Feature
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("buildingId")]
        public string BuildingId { get; set; } = null!;

        [BsonElement("name")]
        public string Name { get; set; } = null!; // np. "Garaż", "Winda"

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("price")]
        public decimal? Price { get; set; } // np. 40000 (dla miejsca garażowego)
    }
}