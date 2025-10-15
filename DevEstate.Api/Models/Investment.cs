using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace DevEstate.Api.Models
{
    public class Investment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("companyId")]
        public string CompanyId { get; set; } = null!;

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("city")]
        public string City { get; set; } = null!;

        [BsonElement("street")]
        public string Street { get; set; } = null!;

        [BsonElement("postalCode")]
        public string PostalCode { get; set; } = null!;

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("status")]
        public string Status { get; set; } = "Aktualne"; // domyślnie aktywna
        
        [BsonElement("images")]
        public List<string> Images { get; set; } = new();
        
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}