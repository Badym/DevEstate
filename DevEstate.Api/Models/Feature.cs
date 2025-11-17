using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DevEstate.Api.Models
{
    public class Feature
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("investmentId")]
        public string InvestmentId { get; set; } = null!;

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("buildingId")]
        public string? BuildingId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("featureTypeId")]
        public string FeatureTypeId { get; set; } = null!;

        [BsonElement("price")]
        public decimal? Price { get; set; }

        [BsonElement("isAvailable")]
        public bool IsAvailable { get; set; } = true;
        
        [BsonElement("isRequired")]
        public bool IsRequired { get; set; } = false;

        [BsonElement("description")]
        public string? Description { get; set; }
        
        
    }
}