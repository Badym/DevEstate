using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace DevEstate.Api.Models
{
    public class Building
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("investmentId")]
        public string InvestmentId { get; set; } = null!;

        [BsonElement("buildingNumber")]
        public string BuildingNumber { get; set; } = null!; // np. "A", "33E"

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("status")]
        public string Status { get; set; } = "Aktualne"; // domyślnie aktywny
        
        [BsonElement("images")]
        public List<string> Images { get; set; } = new();
        
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}