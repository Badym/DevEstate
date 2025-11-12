using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DevEstate.Api.Models
{
    public class FeatureType
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("unitName")]
        public string? UnitName { get; set; }

        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;
    }
}