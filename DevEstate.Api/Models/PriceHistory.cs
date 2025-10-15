using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace DevEstate.Api.Models
{
    public class PriceHistory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("propertyId")]
        public string PropertyId { get; set; } = null!;

        [BsonElement("date")]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [BsonElement("newPrice")]
        public decimal NewPrice { get; set; } // aktualna cena od danej daty
    }
}